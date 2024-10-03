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
                            @click="loadItems"
                    >
                        <v-icon class="pr-1">mdi-refresh</v-icon>
                        Refresh List
                    </v-btn>
                </v-toolbar>
                <v-divider color="teal"></v-divider>
            </template>
            <template #item.fromDate="{item, value}">
                {{value | moment(" MMMM Do YYYY")}} {{item.fromTimeString}}
            </template>
            <template #item.toDate="{value,item}">
                {{value | moment(" MMMM Do YYYY")}} {{item.toTimeString}}
            </template>
            <template #item.status="{value}">
                <v-chip small dark :color="value==='Pending'?'orange darken-1':value==='Rejected'?'error':'teal'"
                >
                    {{value}}
                </v-chip>
            </template>
            <template #item.actions="{ item }">
                <v-icon small
                        class="mr-2"
                        @click="editItem(item)"
                >
                    mdi-eye
                </v-icon>
            </template>
        </v-data-table>
        <v-dialog max-width="800px"
                  scrollable
                  persistent
                  transition="dialog-bottom-transition"
                  v-model="snackbar.dialog"
                  v-if="snackbar.dialog">
            <editForm v-if="snackbar.editForm" :getItemUrl="itemDetailUrl" @closeDialog="closeForm"/>
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
        name: "KajApprovalGrid",
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
                        text: 'Kaj',
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
                    getItemUrl: '',
                    deleteUrl: '',
                    deleteItemTitle: ''
                },
                headers: [
                    {
                        text: 'From (Date & Time)',
                        value: 'fromDate',
                    },
                    {
                        text: 'To (Date & Time)',
                        value: 'toDate',
                    },
                    {
                        text: 'Statement',
                        value: 'remarks',
                    },
                    {
                        text: 'status',
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
                    descending: true,
                    page: 1,
                    itemsPerPage: 10,
                    sortBy: ['kajID'],
                    sortDesc: [true],
                },
                filterParams: {
                    kaj: ''
                }
            }
        },
        methods: {
            editItem({kajID}) {
                this.snackbar.dialog = true
                this.snackbar.editForm = true
                this.snackbar.getItemUrl = 'Kaj/GetKajByID/' + kajID
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
                    Kaj: this.filterParams.kaj,
                }
                try {
                    const {data} = await axios.post("Kaj/KajList", param)
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
            EditForm
        }
    }
</script>

<style scoped>

</style>