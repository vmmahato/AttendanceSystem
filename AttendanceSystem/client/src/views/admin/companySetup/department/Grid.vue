<template>
    <v-container>
        <v-breadcrumbs :items="breadcrumb"></v-breadcrumbs>
        <v-tabs
                v-model="snackbar.tab"
                background-color="transparent"
                color="teal"
                :hide-slider="true"
                right
        >
            <v-tab
                    v-for="item in tabItems"
                    :key="item"
            >
                {{ item }}
            </v-tab>
        </v-tabs>

        <v-tabs-items v-model="snackbar.tab">
            <v-tab-item
                    v-for="item in tabItems"
                    :key="item"
            >
                <v-card color="basil" flat>
                    <SectionGrid v-if="item==='Sections'"></SectionGrid>
                    <v-card-text v-else>
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
                                    >Add Department
                                    </v-btn>
                                </v-toolbar>
                                <v-divider color="teal"></v-divider>
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
                    </v-card-text>
                </v-card>
            </v-tab-item>
        </v-tabs-items>

        <v-dialog max-width="600px"
                  persistent
                  transition="dialog-bottom-transition"
                  v-model="snackbar.dialog"
                  v-if="snackbar.dialog">
            <createForm v-if="snackbar.createForm" @closeDialog="closeForm"/>
            <editForm v-if="snackbar.editForm" :getItemUrl="itemDetailUrl" @closeDialog="closeForm"/>
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
    import SectionGrid from './sections/Grid'
    import EditForm from './Edit'
    import TheDeleteForm from "../../../../components/TheDeleteForm";

    export default {
        name: "DepartmentGrid",
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
                        text: (this.snackbar.tab > 0) ? 'Section' : 'Departments',
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
                    tab: null,
                    dialog: false,
                    createForm: false,
                    editForm: false,
                    deleteForm: false,
                    getItemUrl: '',
                    deleteUrl: '',
                    deleteItemTitle: ''
                },
                headers: [
                    {
                        text: 'Department',
                        value: 'departmentName',
                    },
                    {
                        text: 'Department Code',
                        value: 'departmentCode',
                    },
                    {
                        text: 'Actions',
                        value: 'actions',
                    },
                ],
                rows: [],
                totalRows: 0,
                pagination: {
                    descending: true,
                    page: 1,
                    itemsPerPage: 10,
                    sortBy: ['departmentID'],
                    sortDesc: ['departmentID'],
                },
                filterParams: {
                    departmentName: ''
                },
                tabItems: [
                    'Departments', 'Sections'
                ],
            }
        },
        methods: {
            editItem({departmentID}) {
                this.snackbar.dialog = true
                this.snackbar.editForm = true
                this.snackbar.getItemUrl = 'Department/GetDepartmentByID/' + departmentID
            },
            deleteItem({departmentID, departmentName}) {
                this.snackbar.dialog = true
                this.snackbar.deleteForm = true
                this.snackbar.deleteUrl = 'Department/DeleteDepartment/' + departmentID
                this.snackbar.deleteItemTitle = 'Department ' + '   ' + departmentName
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
                    DepartmentName: this.filterParams.departmentName
                }

                try {
                    const {data} = await axios.post("Department/DepartmentList", param)
                    this.rows = data.data;
                    this.totalRows = data.total;
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
            SectionGrid
        }
    }
</script>

<style scoped>

</style>