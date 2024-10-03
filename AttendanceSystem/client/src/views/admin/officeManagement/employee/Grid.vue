<template>
    <v-container>
        <v-breadcrumbs :items="breadcrumb"></v-breadcrumbs>
        <v-data-table
                :headers="headers"
                :items="rows"
                :server-items-length="totalRows"
                :options.sync="pagination"
                :footer-props="{
                    itemsPerPageOptions: [10, 20, 30, 40, 50]
                                }"
                :loading="snackbar.loading"
                class="elevation-1"
        >
            <template v-slot:top>
                <v-toolbar flat color="white">
                    <v-toolbar-title></v-toolbar-title>
                    <v-spacer></v-spacer>
                    <v-btn
                            small
                            color="teal"
                            dark
                            class="mb-2"
                            @click="createItem"
                    >Add Employee
                    </v-btn>
                </v-toolbar>
                <v-divider color="teal"></v-divider>
            </template>
            <template #item.actions="{ item }">
                <v-icon
                        small
                        @click="showItem(item)"
                        class="mr-2"
                >
                    mdi-eye
                </v-icon>
                <v-icon
                        small
                        class="mr-2"
                        @click="editItem(item)"
                >
                    mdi-pencil
                </v-icon>
                <v-icon
                        small
                        @click="deleteItem(item)"
                >
                    mdi-delete
                </v-icon>
            </template>
            <template #item.activeAccount="{value}">
                <v-icon color="success" v-if="value"> mdi-check-bold</v-icon>
                <v-icon v-else color="error"> mdi-close-thick</v-icon>
            </template>
            <template #item.full_name="{item}">
                <span class="pr-2"> {{item.firstName}}</span>{{item.lastName}}
            </template>
        </v-data-table>
        <v-dialog max-width="900px"
                  persistent
                  scrollable
                  transition="dialog-bottom-transition"
                  v-model="snackbar.dialog"
                  v-if="snackbar.dialog">
            <createForm v-if="snackbar.createForm" @closeDialog="closeForm"/>
            <editForm v-if="snackbar.editForm" :getItemUrl="itemDetailUrl" @closeDialog="closeForm"/>
            <TheDetailGrid v-if="snackbar.detailComponent" :getItemUrl="itemDetailUrl" @closeDialog="closeForm"/>
            <TheDeleteForm v-if="snackbar.deleteForm" :delete-url="deleteUrl" :title="deleteItemTitle"
                           @closeDialog="closeForm"/>
        </v-dialog>
        <v-snackbar :timeout="snackbar.timeOut"
                    :color="snackbar.color"
                    top
                    v-model="snackbar.snackbar">
            {{ snackbar.message }}
            <v-btn @click="snackbar.snackbar = false"
                   dark
                   text>
                Close
            </v-btn>
        </v-snackbar>
    </v-container>
</template>

<script>
    import {mapGetters} from "vuex"
    import axios from "axios";
    import CreateForm from './Create'
    import EditForm from './Edit'
    import TheDetailGrid from './Detail'
    import TheDeleteForm from "../../../../components/TheDeleteForm";

    export default {
        name: "EmployeeGrid",
        computed: {
            ...mapGetters(['getSystemUserData']),
            breadcrumb() {
                return [
                    {
                        disabled: false,
                        exact: true,
                        text: 'Dashboard',
                        to: `${this.getSystemUserData.dashBoard}`
                    },
                    {
                        disabled: true,
                        exact: true,
                        text: 'Employee Lists',
                        to: `${this.getSystemUserData.dashBoard}`
                    }
                ]
            },
            deleteUrl() {
                return this.snackbar.deleteUrl
            },
            deleteItemTitle() {
                return this.snackbar.deleteItemTitle
            },
            itemDetailUrl() {
                return this.snackbar.getItemUrl
            }
        },
        data() {
            return {
                snackbar: {
                    snackbar: false,
                    timeOut: 5000,
                    loading: true,
                    dialog: false,
                    createForm: false,
                    editForm: false,
                    deleteForm: false,
                    detailComponent: false,
                    getItemUrl: '',
                    deleteUrl: '',
                    deleteItemTitle: ''
                },
                headers: [
                    {
                        text: 'Device Number',
                        value: 'deviceNumber',
                    },
                    {
                        text: 'Enroll ID',
                        value: 'enrollID',
                    },
                    {
                        text: "Name",
                        value: "full_name"
                    },
                    {
                        text: 'Department',
                        value: 'departmentName',
                    },
                    {
                        text: 'Section',
                        value: 'sectionName',
                    },
                    {
                        text: 'Designation',
                        value: 'designationName',
                    },
                    {
                        text: 'Contact',
                        value: 'phoneNumber',
                    },
                    {
                        text: 'Active',
                        value: 'activeAccount',
                    },
                    {
                        text: 'Actions',
                        value: 'actions',
                    },
                ],
                rows: [],
                totalRows: 0,
                pagination: {
                    page: 1,
                    itemsPerPage: 10,
                    sortBy: ['employeeID'],
                    sortDesc: [true],
                },
                filterParams: {
                    employeeCode: '',
                    employeeName: '',
                    branchName: '',
                    panNumber: '',
                }
            }
        },
        methods: {
            editItem({employeeID}) {
                this.snackbar.dialog = true
                this.snackbar.editForm = true
                this.snackbar.getItemUrl = 'Employee/GetEmployeeByIDAsync/' + employeeID
            },
            showItem({employeeID}) {
                this.snackbar.dialog = true
                this.snackbar.detailComponent = true
                this.snackbar.getItemUrl = 'Employee/GetEmployeeByIDAsync/' + employeeID
            },
            deleteItem({employeeID, firstName, lastName}) {
                this.snackbar.dialog = true
                this.snackbar.deleteForm = true
                this.snackbar.deleteUrl = 'Employee/DeleteEmployeeAsync/' + employeeID
                this.snackbar.deleteItemTitle = 'Employee ' + '   ' + firstName + '' + lastName
            },
            createItem() {
                this.snackbar.dialog = true
                this.snackbar.createForm = true
            },
            closeForm({message, color}) {
                message ? (this.snackbar.snackbar = true) : (this.snackbar.snackbar = false)
                this.snackbar.dialog = false
                this.snackbar.createForm = false
                this.snackbar.editForm = false
                this.snackbar.deleteForm = false
                this.snackbar.detailComponent = false
                this.snackbar.getItemUrl = ''
                this.snackbar.deleteUrl = ''
                this.snackbar.message = message
                this.snackbar.color = color
                this.loadItems()
            },
            async loadItems() {
                // eslint-disable-next-line no-console
                const {itemsPerPage, page, sortDesc, sortBy} = this.pagination
                let param = {
                    PageSize: itemsPerPage,
                    PageNo: page,
                    OrderType: sortDesc[0] ? 'desc' : 'asc',
                    OrderBy: (sortBy[0] === 'full_name') ? 'firstName' : sortBy[0],
                };

                try {
                    const {data} = await axios.post("Employee/EmployeeListAsync", param)
                    this.rows = data.data;
                    this.totalRows = data.totalCount;
                    this.snackbar.loading = false
                } catch (e) {
                    console.log(e)
                    this.snackbar.loading = false
                }
            },
        },
        created() {
            this.loadItems()
        },
        watch: {
            pagination: {
                handler() {
                    this.loadItems()
                },
                deep: true
            },
        },
        components: {
            TheDeleteForm,
            CreateForm,
            EditForm,
            TheDetailGrid
        }
    }
</script>

<style scoped>

</style>