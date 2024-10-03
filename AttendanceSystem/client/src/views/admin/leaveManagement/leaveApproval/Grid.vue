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
            <template #item.fromDate="{value}">
                {{value | moment(" MMMM Do YYYY")}}
            </template>
            <template #item.toDate="{value}">
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
                    mdi-eye
                </v-icon>
            </template>
        </v-data-table>
        <v-dialog max-width="700px"
                  persistent
                  transition="dialog-bottom-transition"
                  v-model="snackbar.dialog"
                  v-if="snackbar.dialog"
                  scrollable
        >
            <editForm v-if="snackbar.editForm"
                      :getItemUrl="itemDetailUrl"
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
    import EditForm from './Edit'

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
                        text: 'Leave Approval',
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
                        value: 'days',
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
                    employeeName: '',
                    leaveType: '',
                }
            }
        },
        methods: {
            editItem({leaveApplicationID}) {
                this.snackbar.dialog = true
                this.snackbar.editForm = true
                this.snackbar.getItemUrl = 'LeaveApplication/GetLeaveApplicationViewByIDAsync/' + leaveApplicationID
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
                    LeaveName: this.filterParams.employeeName,
                    LeaveCode: this.filterParams.leaveType,
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
            EditForm,
        }
    }
</script>

<style scoped>

</style>