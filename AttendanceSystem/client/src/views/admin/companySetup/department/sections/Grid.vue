<template>
    <v-card color="basil" flat>
        <v-card-text>
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
                        >Add Section
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
    </v-card>
</template>

<script>
    import axios from "axios";
    import CreateForm from './Create'
    import EditForm from './Edit'
    import TheDeleteForm from "../../../../../components/TheDeleteForm";

    export default {
        name: "SectionGrid",
        computed: {
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
                        text: 'Section',
                        value: 'sectionName',
                    },
                    {
                        text: 'Section Code',
                        value: 'sectionCode',
                    },
                    {
                        text: 'Department',
                        value: 'departmentName',
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
                    sortBy: ['sectionID'],
                    sortDesc: ['true'],
                },
                filterParams: {
                    section: ''
                },
            }
        },
        methods: {
            editItem({sectionID}) {
                this.snackbar.dialog = true
                this.snackbar.editForm = true
                this.snackbar.getItemUrl = 'Section/GetSectionByID/' + sectionID
            },
            deleteItem({sectionID, sectionName}) {
                this.snackbar.dialog = true
                this.snackbar.deleteForm = true
                this.snackbar.deleteUrl = 'Section/DeleteSection/' + sectionID
                this.snackbar.deleteItemTitle = 'Section ' + '   ' + sectionName
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
                    FiscalYear: this.filterParams.section
                }

                try {
                    const {data} = await axios.post("Section/SectionList", param)
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