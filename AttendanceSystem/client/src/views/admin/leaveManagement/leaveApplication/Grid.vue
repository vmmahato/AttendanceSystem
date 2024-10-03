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
                    >Create Application
                    </v-btn>
                </v-toolbar>
                <v-divider color="teal"></v-divider>
            </template>
            <template #item.toDate="{value}">
                {{value | moment(" MMMM Do YYYY")}}
            </template>
            <template #item.fromDate="{value}">
                {{value | moment(" MMMM Do YYYY")}}
            </template>
            <template #item.status="{value}">
                <v-chip small dark :color="value==='Pending'?'orange darken-1':value==='Rejected'?'error':'teal'"
                >
                    {{value}}
                </v-chip>
            </template>
            <template #item.actions="{ item }">
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
        </v-data-table>
        <v-dialog max-width="800px"
                  persistent
                  scrollable
                  transition="dialog-bottom-transition"
                  v-model="snackbar.dialog"
                  v-if="snackbar.dialog">
            <createForm v-if="snackbar.createForm"
                        @closeDialog="closeForm"/>
            <editForm v-if="snackbar.editForm"
                      :getItemUrl="itemDetailUrl"
                      @closeDialog="closeForm"/>
            <TheDeleteForm v-if="snackbar.deleteForm"
                           :delete-url="deleteUrl"
                           :title="deleteItemTitle"
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
    import TheDeleteForm from "../../../../components/TheDeleteForm";

    export default {
        name: "LeaveGrid",
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
                        text: 'Leave Application',
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
                        text: 'Employee',
                        value: 'employeeName',
                    },
                    {
                        text: 'Leave Type',
                        value: 'leaveType',
                    },
                    {
                        text: 'From',
                        value: 'fromDate',
                    },
                    {
                        text: 'To',
                        value: 'toDate',
                    },
                    {
                        text: 'Days',
                        value: 'isReplacementLeave',
                    },
                    {
                        text: 'Leave Day',
                        value: 'leaveDay',
                    },
                    {
                        text: 'Approved By',
                        value: 'approvedByName',
                    },
                    {
                        text: 'Status',
                        value: 'status',
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
                    sortBy: ['leaveApplicationID'],
                    sortDesc: [true],
                },
                filterParams: {
                    FirstName: '',
                    LastName: '',
                }
            }
        },
        methods: {
            editItem({leaveApplicationID}) {
                this.snackbar.dialog = true
                this.snackbar.editForm = true
                this.snackbar.getItemUrl = 'LeaveApplication/GetLeaveApplicationByID/' + leaveApplicationID
            },
            deleteItem({leaveApplicationID, employeeName}) {
                this.snackbar.dialog = true
                this.snackbar.deleteForm = true
                this.snackbar.deleteUrl = 'LeaveApplication/DeleteLeaveApplication/' + leaveApplicationID
                this.snackbar.deleteItemTitle = 'Leave Application ' + ' for  ' + employeeName
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
                    OrderBy: sortBy[0],
                    LeaveName: this.filterParams.leaveName,
                    LeaveCode: this.filterParams.leaveCode,
                }
                try {
                    const {data} = await axios.post("LeaveApplication/LeaveApplicationList", param)
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
        }
    }
</script>

<style scoped>

</style>