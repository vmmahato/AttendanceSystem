<template>
    <v-container>
        <v-row>
            <v-col cols="12" sm="9">
                <section class="highlightSection">
                    <v-row>
                        <v-col cols="12" sm="3" v-for="(item,index) in highlightCards" :key="index">
                            <v-card
                                    dark
                                    class="mx-auto"
                                    max-width="344"
                                    outlined
                                    :style="item.style"
                            >
                                <v-list-item three-line>
                                    <v-list-item-content>
                                        <v-list-item-title class="headline mb-1">{{item.count}}</v-list-item-title>
                                        <v-list-item-subtitle>{{item.title}}</v-list-item-subtitle>
                                    </v-list-item-content>
                                    <v-list-item-icon>
                                        <v-icon>{{item.icon}}</v-icon>
                                    </v-list-item-icon>
                                </v-list-item>
                            </v-card>
                        </v-col>
                    </v-row>
                </section>
                <section>
                    <v-row>
                        <v-col cols="12" sm="6">
                            <TheSimpleTableComponent classes="onLeaveCard"
                                                     title="On Leave"
                                                     height="200px"
                                                     header-type="fixed-header"
                                                     :header="leaveTableHeader"
                                                     :items="leaveTableData"
                                                     tableKey="leaveTable"
                            ></TheSimpleTableComponent>
                        </v-col>
                        <v-col cols="12" sm="6">
                            <TheSimpleTableComponent classes="absentCard"
                                                     title="Absent"
                                                     height="200px"
                                                     header-type="fixed-header"
                                                     :header="absentTableHeader"
                                                     :items="absentTableData"
                                                     tableKey="absentTable"
                            ></TheSimpleTableComponent>
                        </v-col>
                        <v-col cols="12" sm="6">
                            <TheSimpleTableComponent classes="upComingCard"
                                                     title="Upcoming Holidays"
                                                     height="220px"
                                                     header-type="fixed-header"
                                                     :header="holidayTableHeader"
                                                     :items="holidayTableData"
                                                     tableKey="upComingTable"
                            ></TheSimpleTableComponent>
                        </v-col>
                        <v-col cols="12" sm="6">
                            <TheSimpleTableComponent classes="officialVisitsCard"
                                                     title="Office Visits"
                                                     height="220px"
                                                     header-type="fixed-header"
                                                     :header="visitorTableHeader"
                                                     :items="visitorTableData"
                                                     tableKey="officeVisitTable"
                            ></TheSimpleTableComponent>
                        </v-col>
                    </v-row>
                </section>
            </v-col>
            <v-col cols="12" sm="3">
                <section class="employeeStatus">
                    <v-card elevation="1">
                        <v-card-title class="body-1 pb-1">Employee Status</v-card-title>
                        <v-card-text class="subtitle-1">
                            <v-simple-table>
                                <template v-slot:default>
                                    <tbody>
                                    <tr class="text-left">
                                        <td>Present</td>
                                        <td>{{ employeeStatusData.presentCount }}</td>
                                    </tr>
                                    <tr class="text-left">
                                        <td>Late</td>
                                        <td>{{ employeeStatusData.lateCount }}</td>
                                    </tr>
                                    <tr class="text-left">
                                        <td>Office Visit</td>
                                        <td>{{ employeeStatusData.officeVisitCount }}</td>
                                    </tr>
                                    <tr class="text-left">
                                        <td>Kaj</td>
                                        <td>{{ employeeStatusData.kajCount }}</td>
                                    </tr>
                                    <tr class="text-left">
                                        <td>On Leave</td>
                                        <td>{{ employeeStatusData.leaveCount }}</td>
                                    </tr>
                                    <tr class="text-left">
                                        <td>Absent</td>
                                        <td>{{ absentCount }}</td>
                                    </tr>
                                    </tbody>
                                </template>
                            </v-simple-table>
                        </v-card-text>
                    </v-card>
                </section>
                <section class="devices pt-10">
                    <v-card depressed class="devicesCard">
                        <v-card-title class="body-1 pb-1">Devices</v-card-title>
                        <v-card-text class="subtitle-1">
                            <v-simple-table fixed-header height="250">
                                <template v-slot:default>
                                    <thead>
                                    <tr>
                                        <th class="text-left teal--text">Device Number</th>
                                        <th class="text-left teal--text">Status</th>
                                    </tr>
                                    </thead>
                                    <tbody>
                                    <tr v-for="item in devicesData" :key="item.name" class="text-left">
                                        <td>{{ item.deviceNumber }}</td>
                                        <td>{{ item.status }}</td>
                                    </tr>
                                    </tbody>
                                </template>
                            </v-simple-table>
                        </v-card-text>
                    </v-card>
                </section>
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="12" sm="8">
                <TheSimpleTableComponent classes="deviceLogCard"
                                         title="Device Log"
                                         height="200px"
                                         header-type="fixed-header"
                                         :header="deviceLogHeader"
                                         :items="deviceLogs"
                                         tableKey="deviceLogTable"
                ></TheSimpleTableComponent>

            </v-col>
            <v-col cols="12" sm="4">
                <TheSimpleTableComponent classes="activityLogCard"
                                         title="Activity Log"
                                         height="200px"
                                         header-type="fixed-header"
                                         :header="activityLogHeader"
                                         :items="activityLogs"
                                         tableKey="activityLogTable"
                ></TheSimpleTableComponent>
            </v-col>
        </v-row>
    </v-container>
</template>

<script>
    import TheSimpleTableComponent from "../../TheSimpleTableComponent";
    import axios from 'axios'

    export default {
        name: 'TheAdminDashboard',

        data() {
            return {
                highlightCards: [
                    {
                        title: 'Total Department',
                        count: 0,
                        icon: 'mdi-domain',
                        link: '',
                        style: 'background: linear-gradient(to right, rgb(50, 50, 87), rgb(50, 74, 74)); box-shadow: rgba(52, 56, 85, 0.25) 0px 2px 3px 0px;'
                    },
                    {
                        title: 'Employees',
                        count: 0,
                        icon: 'mdi-account-multiple',
                        link: '',
                        style: 'color: rgb(255, 255, 255); background: linear-gradient(to right, rgb(52, 181, 229), rgb(46, 132, 224)); box-shadow: rgba(52, 56, 85, 0.25) 0px 2px 3px 0px;'
                    },
                    {
                        title: 'Active',
                        count: 0,
                        icon: 'mdi-account-multiple-check-outline',
                        link: '',
                        style: 'color: rgb(255, 255, 255); background: linear-gradient(to right, rgb(80, 191, 149), rgb(15, 199, 80)); box-shadow: rgba(52, 56, 85, 0.25) 0px 2px 3px 0px;'
                    },
                    {
                        title: 'Absent',
                        count: 0,
                        icon: 'mdi-account-multiple-remove-outline',
                        link: '',
                        style: 'color: rgb(255, 255, 255); background: linear-gradient(to right, rgb(255, 118, 87), rgb(227, 74, 74)); box-shadow: rgba(52, 56, 85, 0.25) 0px 2px 3px 0px;'
                    },
                ],
                deviceLogs: [],
                deviceLogHeader: [
                    {
                        text: 'Name',
                        value: 'employeeName'
                    },
                    {
                        text: 'Activity',
                        value: 'dateStatus'
                    },
                    {
                        text: 'Time',
                        value: 'puntchdate'
                    },
                    {
                        text: 'Machine',
                        value: 'deviceNumber'
                    },

                ],
                activityLogs: [],
                activityLogHeader: [
                    {
                        text: 'Name',
                        value: 'employeeName',
                    },
                    {
                        text: 'Status',
                        value: 'startDateStatus',
                    },
                ],
                leaveTableData: [],
                leaveTableHeader: [
                    {
                        text: 'Name',
                        value: 'name',
                    },
                    {
                        text: 'Department',
                        value: 'department',
                    },
                ],
                holidayTableData: [],
                holidayTableHeader: [
                    {
                        text: 'Name',
                        value: 'name',
                    },
                    {
                        text: 'Type',
                        value: 'type',
                    },
                    {
                        text: 'From',
                        value: 'fromDate',
                    },
                    {
                        text: 'To',
                        value: 'toDate',
                    },
                ],
                visitorTableData: [],
                visitorTableHeader: [
                    {
                        text: 'Visitor',
                        value: 'visitor',
                    },
                    {
                        text: 'Employee',
                        value: 'employee',
                    },
                    {
                        text: 'From',
                        value: 'fromDate',
                    },
                    {
                        text: 'To',
                        value: 'toDate',
                    },
                ],
                absentTableData: [],
                absentTableHeader: [
                    {
                        text: 'Name',
                        value: 'name',
                    },
                    {
                        text: 'Department',
                        value: 'department',
                    },
                ],
                devicesData: [],
                employeeStatusData: {},
                absentCount: null
            }
        },
        methods: {
            async getLeaveTableData() {
                try {
                    const {data} = await axios.get('Shared/GetLeaveCount')
                    this.leaveTableData = data
                } catch (e) {
                    console.log(e)
                }

            },
            async getDeviceLogsTableData() {
                try {
                    const {data} = await axios.get('Shared/GetAttendanceLogCount')
                    this.deviceLogs = data
                } catch (e) {
                    console.log(e)
                }

            },
            async getActivityLogsTableData() {
                try {
                    const {data} = await axios.get('Shared/GetAttendanceStatusCount')
                    this.activityLogs = data
                } catch (e) {
                    console.log(e)
                }

            },
            async getDevicesData() {
                try {
                    const {data} = await axios.get('Shared/GetDeviceStatusCount')
                    this.devicesData = data
                } catch (e) {
                    console.log(e)
                }

            },
            async getHolidayTableData() {
                try {
                    const {data} = await axios.get('Shared/GetHolidayCount')
                    this.holidayTableData = data
                } catch (e) {
                    console.log(e)
                }

            },
            async getAbsentTableData() {
                try {
                    const {data} = await axios.get('Shared/GetAbsentCount')
                    this.absentTableData = data.list
                    this.absentCount = data.totalAbsentCount
                    this.highlightCards[3].count = data.totalAbsentCount
                } catch (e) {
                    console.log(e)
                }

            },
            async getVisitorTableData() {
                try {
                    const {data} = await axios.get('Shared/GetVisitorCount')
                    this.visitorTableData = data
                } catch (e) {
                    console.log(e)
                }

            },
            async getEmployeeStatusTableData() {
                try {
                    const {data} = await axios.get('Shared/GetStatus')
                    this.employeeStatusData = data
                } catch (e) {
                    console.log(e)
                }

            },
            async getHighlightCardData() {
                try {
                    const {data} = await axios.get('Shared/GetCounts')
                    this.highlightCards[0].count = data.departmentCount
                    this.highlightCards[1].count = data.employeesCount
                    this.highlightCards[2].count = data.activeUserCount
                } catch (e) {
                    console.log(e)
                }

            },
        },
        created() {
            this.getLeaveTableData()
            this.getAbsentTableData()
            this.getHolidayTableData()
            this.getVisitorTableData()
            this.getEmployeeStatusTableData()
            this.getHighlightCardData()
            this.getDevicesData()
            this.getDeviceLogsTableData()
            this.getActivityLogsTableData()
        },
        components: {
            TheSimpleTableComponent
        }
    }
</script>
<style scoped lang="scss">
    $grey1: #eeeeee99;
    /* width */
    ::-webkit-scrollbar {
        width: 6px;
    }

    /* Track */
    ::-webkit-scrollbar-track {
        background: $grey1;
    }

    /* Handle */
    ::-webkit-scrollbar-thumb {
        background: #4e434361;
    }

    /* Handle on hover */
    ::-webkit-scrollbar-thumb:hover {
        background: #4e434361;
    }

    .v-data-table, .v-data-table--fixed-header, .v-data-table__wrapper {
        /* width */
        ::-webkit-scrollbar {
            width: 6px;
        }

        /* Track */
        ::-webkit-scrollbar-track {
            background: $grey1;
        }

        /* Handle */
        ::-webkit-scrollbar-thumb {
            background: #4e434361;
        }

        /* Handle on hover */
        ::-webkit-scrollbar-thumb:hover {
            background: #4e434361;
        }
    }

    .onLeaveCard {
        border-top: 4px solid teal;
    }

    .absentCard {
        border-top: 4px solid darkred;
    }

    .upComingCard {
        border-top: 4px solid rebeccapurple;
    }

    .officialVisitsCard {
        border-top: 4px solid lightseagreen;
    }

    .deviceLogCard {
        border-top: 4px solid cyan;
    }

    .activityLogCard {
        border-top: 4px solid indigo;
    }

    .devicesCard {
        border-top: 4px solid purple;
    }
</style>
