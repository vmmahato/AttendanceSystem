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
                    >Add Device
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
        </v-data-table>
        <v-dialog max-width="700px"
                  persistent
                  transition="dialog-bottom-transition"
                  v-model="snackbar.dialog"
                  v-if="snackbar.dialog">
            <createForm v-if="snackbar.createForm" @closeDialog="closeForm"/>
            <TheDetailGrid v-if="snackbar.detailComponent" :getItemUrl="itemDetailUrl" @closeDialog="closeForm"/>
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
    import EditForm from './Edit'
    import TheDetailGrid from './Detail'
    import TheDeleteForm from "../../../../components/TheDeleteForm";

    export default {
        name: "DeviceGrid",
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
                        text: 'Device Manager',
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
                        text: 'Device Name',
                        value: 'deviceName',
                    }, 
                    {
                        text: 'Device Number',
                        value: 'deviceNumber',
                    },
                    {
                        text: 'Communication Mode',
                        value: 'communicationMode',
                    },
                    {
                        text: 'IP Address',
                        value: 'ipAddress',
                    },
                    {
                        text: 'Port',
                        value: 'communicationPort',
                    },
                    {
                        text: 'Serial Number',
                        value: 'serialNumber',
                    },
                    {
                        text: 'is Active',
                        value: 'isActive',
                    },
                    {
                        text: 'is Registered',
                        value: 'isRegister',
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
                    sortBy: ['deviceID'],
                    sortDesc: ['deviceID'],
                },
                filterParams: {
                    deviceName: '',
                    deviceNumber: '',
                    deviceModelID: '',
                }
            }
        },
        methods: {
            showItem({deviceID}) {
                this.snackbar.dialog = true
                this.snackbar.detailComponent = true
                this.snackbar.getItemUrl = 'Device/GetDeviceByID/' + deviceID
            },
            editItem({deviceID}) {
                this.snackbar.dialog = true
                this.snackbar.editForm = true
                this.snackbar.getItemUrl = 'Device/GetDeviceByID/' + deviceID
            },
            deleteItem({deviceID, deviceName}) {
                this.snackbar.dialog = true
                this.snackbar.deleteForm = true
                this.snackbar.deleteUrl = 'Device/DeleteDevice/' + deviceID
                this.snackbar.deleteItemTitle = 'Device ' + '   ' + deviceName
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
                this.snackbar.detailComponent = false
                this.snackbar.deleteForm = false
                this.snackbar.getItemUrl = ''
                this.snackbar.deleteUrl = ''
                this.snackbar.message = message
                this.snackbar.color = color
                this.loadItems()
            },
            async loadItems() {
                // eslint-disable-next-line no-console

                let param = {
                    PageSize: this.pagination.itemsPerPage,
                    PageNo: this.pagination.page,
                    OrderType: this.pagination.descending ? 'desc' : 'asc',
                    OrderBy: this.pagination.sortBy[0],
                    DeviceName: this.filterParams.deviceName,
                    DeviceNumber: this.filterParams.deviceNumber,
                    deviceModelID: this.filterParams.deviceModelID,
                };

                try {
                    const {data} = await axios.post("Device/DeviceList", param)
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
            TheDetailGrid,
        }
    }
</script>

<style scoped>

</style>