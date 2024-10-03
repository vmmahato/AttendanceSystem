<template>
    <v-container class="reportClass">
        <v-card class="customFontSize">
            <v-card-title></v-card-title>

            <v-card-text>
                <v-row justify="end" no-gutters>
                    <v-col sm="auto" class="ma-0 pa-0 px-1">
                        <v-btn dark color="indigo darken-1" outlined @click="exportTableToExcel('simpleTable')">Download
                            Excel
                        </v-btn>
                    </v-col>
                    <v-col sm="auto" class="ma-0 pa-0 px-1">
                        <v-btn dark color="error darken-1" outlined @click="PrintDiv">Print</v-btn>
                    </v-col>
                    <v-col sm="auto" class="ma-0 pa-0">
                        <v-autocomplete v-model="selectedHeaders" :items="headersList" label="Select Columns" multiple
                                        return-object
                                        class="ma-0 pa-0"
                        >
                            <template v-slot:selection="{ item, index }">
                                <v-chip v-if="index < 2">
                                    <span>{{ item.text }}</span>
                                </v-chip>
                                <span v-if="index === 2"
                                      class="grey--text caption">(+{{ selectedHeaders.length - 2 }} others)</span>
                            </template>
                        </v-autocomplete>
                    </v-col>
                </v-row>
                <v-simple-table id="simpleTable" class="">
                    <template v-slot:default>
                        <thead class="table-bordered" style="background-color: teal; color: #eee">
                        <tr>
                            <th class="text-center" v-for="(item, index) in tableHeaders"
                                :key="index"
                                :rowspan="item.rowspan"
                                :colspan="item.children.length||1"
                                style="color: #eeeeee"
                            >{{item.text}}
                            </th>
                        </tr>
                        <tr>
                            <template v-for="(item,index) in tableHeaders">
                                <template v-if="item.children">
                                    <td v-for="(td,count) in item.children"
                                        :key="'subheader'+index+count"
                                        style="color: #eeeeee"
                                    >{{td.text}}
                                    </td>
                                </template>
                            </template>
                        </tr>
                        </thead>
                        <tbody>
                        <tr v-for="(item,row) in dataList" :key="'dataKey'+row">
                            <template v-for="(value,index) in selectedHeaders">
                                <td :key="'row'+row+'column'+index">
                                    {{item[value.value]}}
                                </td>
                            </template>
                        </tr>
                        </tbody>
                    </template>
                </v-simple-table>
            </v-card-text>
        </v-card>
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

    export default {
        name: "ReportComponent",
        props: {
            employeeID: {
                type: Array,
            },
            report: {
                type: String,
            },
            fromDate: {
                type: String,
            },
            toDate: {
                type: String,
            },
            month: {
                type: Number,
            },
            fiscalYear: {
                type: Number,
            },
        },
        computed: {
            ...mapGetters(['getSystemUserData']),
            reportType() {
                return this.report
            },
            reportEmployeeList() {
                return this.employeeID
            },
            reportFromDate() {
                return this.fromDate
            },
            reportToDate() {
                return this.toDate
            },
            reportMonth() {
                return this.month
            },
            reportFiscalYear() {
                return this.fiscalYear
            },
            tableHeaders() {
                return this.tableHeader.reduce((acc, item) => {
                    const ch = item.children && item.children.filter(child => this.selectedHeaders.some(header => header.value === child.value))
                    if (ch && ch.length) acc.push({...item, children: ch})
                    else if (this.selectedHeaders.some(header => header.value === item.value)) acc.push({...item})
                    return acc;
                }, [])
            },
        },
        data() {
            return {
                snackbar: {
                    snackbar: false,
                    timeOut: 5000,
                    dateMenu1: false,
                    loading: true,
                    dialog: false,
                    reportComponent: false,
                    editForm: false,
                    deleteForm: false,
                    getItemUrl: '',
                    deleteUrl: '',
                    deleteItemTitle: '',
                    employeeID: [],
                },
                tableHeader: [
                    {
                        text: 'Employee Name',
                        value: 'name',
                        children: [
                        ],
                        rowspan: 2,
                        show: true
                    },
                    {
                        text: 'From',
                        value: 'fromDate',
                        children: [
                        ],
                        rowspan: 1,
                        show: true
                    },
                    {
                        text: 'To',
                        value: 'toDate',
                        children: [
                        ],
                        rowspan: 1,
                        show: true
                    },
                    {
                        text: 'Leave Day',
                        value: 'leaveDay',
                        children: [],
                        rowspan: 2,
                        show: true
                    },
                    {
                        text: 'Status',
                        value: 'status',
                        children: [

                        ],
                        rowspan: 1,
                        show: true
                    },
                    {
                        text: 'Leave Name',
                        value: 'leaveName',
                        children: [

                        ],
                        rowspan: 1,
                        show: true
                    },
                    {
                        text: 'Remarks',
                        value: 'replacementLeaveType',
                        children: [],
                        rowspan: 2,
                        show: true
                    },
                ],
                selectedHeaders: [

                    {
                        text: 'Name',
                        value: 'name'
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
                        text: 'Leave Day',
                        value: 'leaveDay',
                    },
                    {
                        text: 'Status',
                        value: 'status',
                    },
                    {
                        text: 'Leave Name',
                        value: 'leaveName',
                    },
                    {
                        text: 'Remarks',
                        value: 'replacementLeaveType',
                    },
                ],
                headersList: [

                    {
                        text: 'Name',
                        value: 'name'
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
                        text: 'Leave Day',
                        value: 'leaveDay',
                    },
                    {
                        text: 'Status',
                        value: 'status',
                    },
                    {
                        text: 'Leave Name',
                        value: 'leaveName',
                    },
                    {
                        text: 'Remarks',
                        value: 'replacementLeaveType',
                    },
                ],
                dataList: [],
            }
        },
        methods: {
            async loadItems() {
                // eslint-disable-next-line no-console
                this.snackbar.loading = true
                try {
                    const {data} = await axios.post("Report/MonthlyLeaveReport", {
                        "ReportType": this.reportType,
                        "EmployeeID": this.reportEmployeeList,
                        "FromDate": this.reportFromDate,
                        "ToDate": this.reportToDate,
                        "Month": this.reportMonth,
                        "FiscalYearID": this.reportFiscalYear,
                    })
                    this.dataList = data;
                    this.snackbar.loading = false
                } catch (e) {
                    console.log(e)
                    this.snackbar.loading = false
                }
            },
            exportTableToExcel(tableID, filename = '') {
                let downloadLink;
                let dataType = 'application/vnd.ms-excel';
                let tableSelect = document.getElementById(tableID);
                let tableHTML = tableSelect.outerHTML.replace(/ /g, '%20');

                // Specify file name
                filename = filename ? filename + '.xls' : 'excel_data.xls';

                // Create download link element
                downloadLink = document.createElement("a");

                document.body.appendChild(downloadLink);

                if (navigator.msSaveOrOpenBlob) {
                    let blob = new Blob(['\ufeff', tableHTML], {
                        type: dataType
                    });
                    navigator.msSaveOrOpenBlob(blob, filename);
                } else {
                    // Create a link to the file
                    downloadLink.href = 'data:' + dataType + ', ' + tableHTML;

                    // Setting the file name
                    downloadLink.download = filename;

                    //triggering the function
                    downloadLink.click();
                }
            },
            PrintDiv() {
                let borderClass = ".table-bordered tr th, td { border: 1px solid #ccc;padding:1rem}"
                let widthClass = ".table-bordered { width:100%}"
                let divContents = document.getElementById("simpleTable").innerHTML;
                let divTitle = "<h3>Report Daily Attendance</h3>";
                let printWindow = window.open("");
                printWindow.document.write('<html><head><title>Report Daily Attendance</title>');
                printWindow.document.write('<style type="text/css" media="print">' +
                    borderClass +
                    widthClass+
                    '</style>');
                printWindow.document.write('</head><body >');
                printWindow.document.write(divTitle);
                printWindow.document.write(divContents);
                printWindow.document.write('</body></html>');
                printWindow.document.close();
                printWindow.print();
            }
        },
        created() {
            this.loadItems()
        },
        watch: {
            employeeID: {
                handler() {
                    this.loadItems()
                },
                deep: true,
                immediate: false
            },
            fromDate: {
                handler() {
                    this.loadItems()
                },
                deep: true,
                immediate: false
            },
            toDate: {
                handler() {
                    this.loadItems()
                },
                deep: true,
                immediate: false
            },
            report: {
                handler() {
                    this.loadItems()
                },
                deep: true,
                immediate: false
            },
        }
    }
</script>

<style scoped lang="scss">
    .borderClass {
        border-top: 4px solid lightseagreen;
    }
    .table-bordered tr th, td {
        border: 1px solid #ccc;
    }

    .reportClass {
        .customFontSize .v-card__actions .v-text-field input, .customFontSize .v-card__actions .v-input--selection-controls input {
            font-size: 0.8em;
            text-transform: capitalize;
        }

        .customFontSize .v-card__actions .v-text-field label, .customFontSize .v-card__actions .v-list-item__content .v-list-item__title, .customFontSize .v-input--selection-controls label {
            font-size: 0.8em !important;
        }
    }
</style>