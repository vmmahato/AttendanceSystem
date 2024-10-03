<template>
  <v-container>
    <v-breadcrumbs :items="breadcrumb"></v-breadcrumbs>
    <v-data-table
      :headers="headers"
      :items="rows"
      :server-items-length="totalRows"
      :options.sync="pagination"
      :footer-props="{
        itemsPerPageOptions: [10, 20, 30, 40, 50],
      }"
      :loading="snackbar.loading"
      class="elevation-1"
    >
      <template v-slot:top>
        <v-toolbar flat color="white">
          <v-toolbar-title></v-toolbar-title>
          <v-spacer></v-spacer>
          <v-btn small color="teal" dark class="mb-2" @click="createItem"
            >Add Work shift
          </v-btn>
        </v-toolbar>
        <v-divider color="teal"></v-divider>
      </template>
      <template #item.shiftEndString="{ value }">
        <span v-if="value">  {{ value | timeFormat }} </span>
      </template>
      <template #item.shiftStartString="{ value }">
        <span v-if="value">  {{ value | timeFormat }} </span>
      </template>
      <template #item.lateInString="{ value }">
        <span v-if="value"> {{ value | timeFormat }} </span> 
      </template>
      <template #item.isMustCheckOut="{ value }">
        <v-icon color="success" v-if="value"> mdi-check-bold</v-icon>
        <v-icon v-else color="error"> mdi-close-thick</v-icon>
      </template>
      <template #item.isEarlyLeave="{ value }">
        <v-icon color="success" v-if="value"> mdi-check-bold</v-icon>
        <v-icon v-else color="error"> mdi-close-thick</v-icon>
      </template>
      <template #item.isMustCheckIn="{ value }">
        <v-icon color="success" v-if="value"> mdi-check-bold</v-icon>
        <v-icon v-else color="error"> mdi-close-thick</v-icon>
      </template>
      <template #item.actions="{ item }">
        <v-icon small class="mr-2" @click="editItem(item)"> mdi-pencil </v-icon>
        <v-icon small @click="deleteItem(item)"> mdi-delete </v-icon>
      </template>
    </v-data-table>
    <v-dialog
      max-width="900px"
      persistent
      transition="dialog-bottom-transition"
      v-model="snackbar.dialog"
      v-if="snackbar.dialog"
    >
      <createForm v-if="snackbar.createForm" @closeDialog="closeForm" />
      <editForm
        v-if="snackbar.editForm"
        :getItemUrl="itemDetailUrl"
        @closeDialog="closeForm"
      />
      <TheDeleteForm
        v-if="snackbar.deleteForm"
        :delete-url="deleteUrl"
        :title="deleteItemTitle"
        @closeDialog="closeForm"
      />
    </v-dialog>
    <v-snackbar
      :timeout="snackbar.timeOut"
      :color="snackbar.color"
      top
      v-model="snackbar.snackbar"
    >
      {{ snackbar.message }}
      <v-btn @click="snackbar.snackbar = false" dark text> Close </v-btn>
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
        name: "WorkShiftsGrid",
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
                        text: 'Work Shifts',
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
        filters: {
            timeFormat: function(date) {
               if (typeof date === "string") {
            let [hours, minutes] = date.split(":");
            let ampm = "AM";

            if (Number(hours) > 12) {
            hours = Number(hours) - 12;
            ampm = "PM";
            }

            return `${hours}:${minutes} ${ampm}`;

        } else if (date instanceof Date) {
            let hours = date.getHours();
            let minutes = date.getMinutes();

            let ampm = hours >= 12 ? "PM" : "AM";

            hours = hours % 12;
            hours = hours ? hours : 12; // the hour '0' should be '12'
            minutes = minutes < 10 ? "0" + minutes : minutes;

            let strTime = hours + ":" + minutes + " " + ampm;

            return strTime;
             }

            return date;
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
                        text: 'Shift Code',
                        value: 'shiftCode',
                    },
                    {
                        text: 'Shift Name',
                        value: 'shiftName',
                    },
                    {
                        text: 'Shift Start',
                        value: 'shiftStartString',
                    },
                    {
                        text: 'Shift End',
                        value: 'shiftEndString',
                    },
                    {
                        text: 'Late Time',
                        value: 'lateInString',
                    },
                    {
                        text: 'Leave Early',
                        value: 'isEarlyLeave',
                    },
                    {
                        text: 'Check In',
                        value: 'isMustCheckIn',
                    },
                    {
                        text: 'Check Out',
                        value: 'isMustCheckOut',
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
                    sortBy: ['shiftID'],
                    sortDesc: [true],
                },
                filterParams: {
                    shiftName: ''
                },
            }
        },
        methods: {
            editItem({shiftID}) {
                this.snackbar.dialog = true
                this.snackbar.editForm = true
                this.snackbar.getItemUrl = 'WorkShift/GetWorkShiftTypeByIDAsync/' + shiftID
            },
            deleteItem({shiftID, shiftName}) {
                this.snackbar.dialog = true
                this.snackbar.deleteForm = true
                this.snackbar.deleteUrl = 'WorkShift/DeleteWorkShiftAsync/' + shiftID
                this.snackbar.deleteItemTitle = 'Shift ' + '   ' + shiftName
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
                }
                try {
                    const {data} = await axios.post("WorkShift/WorkShiftListAsync", param)
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
            EditForm
        }
    }
</script>

<style scoped>
</style>