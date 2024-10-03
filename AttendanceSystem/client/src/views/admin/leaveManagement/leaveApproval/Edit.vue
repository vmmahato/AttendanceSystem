<template>
    <v-card class="text-left">
        <v-card-title>
            <span class="headline">Leave Approval</span>
            <v-spacer></v-spacer>
            <v-btn depressed flat text @click="closeForm">
                <v-icon>mdi-close</v-icon>
            </v-btn>
        </v-card-title>

        <v-card-text>
            <v-snackbar :color="snackbar.color"
                        top
                        :value="snackbar.snackbar">
                {{ snackbar.message }}
                <v-btn @click="snackbar.snackbar = false"
                       dark
                       text>
                    Close
                </v-btn>
            </v-snackbar>
            <v-container>
                <v-row no-gutters>
                    <v-col cols="12" md="6" >
                        <v-list three-line subheader class="ma-0 pa-0">
                            <v-list-item >
                                <v-list-item-content>
                                    <v-list-item-title>Code</v-list-item-title>
                                    <v-list-item-subtitle>{{formData.code}}</v-list-item-subtitle>
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-col>
                    <v-col cols="12" md="6" >
                        <v-list three-line subheader class="ma-0 pa-0">
                            <v-list-item >
                                <v-list-item-content>
                                    <v-list-item-title>Leave Type</v-list-item-title>
                                    <v-list-item-subtitle>{{formData.leaveType}}</v-list-item-subtitle>
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-col>
                    <v-col cols="12" md="6" >
                        <v-list three-line subheader class="ma-0 pa-0">
                            <v-list-item >
                                <v-list-item-content>
                                    <v-list-item-title>Designation</v-list-item-title>
                                    <v-list-item-subtitle>{{formData.designationName}}</v-list-item-subtitle>
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-col>
                    <v-col cols="12" md="6" >
                        <v-list three-line subheader class="ma-0 pa-0">
                            <v-list-item >
                                <v-list-item-content>
                                    <v-list-item-title>Leave Day</v-list-item-title>
                                    <v-list-item-subtitle>{{formData.leaveDay}}</v-list-item-subtitle>
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-col>
                    <v-col cols="12" md="6" >
                        <v-list three-line subheader class="ma-0 pa-0">
                            <v-list-item >
                                <v-list-item-content>
                                    <v-list-item-title>Department</v-list-item-title>
                                    <v-list-item-subtitle>{{formData.departmentName}}</v-list-item-subtitle>
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-col>
                    <v-col cols="12" md="6" >
                        <v-list three-line subheader class="ma-0 pa-0">
                            <v-list-item >
                                <v-list-item-content>
                                    <v-list-item-title>Section</v-list-item-title>
                                    <v-list-item-subtitle>{{formData.sectionName}}</v-list-item-subtitle>
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-col>
                    <v-col cols="12" md="6" >
                        <v-list three-line subheader class="ma-0 pa-0">
                            <v-list-item >
                                <v-list-item-content>
                                    <v-list-item-title>From Date</v-list-item-title>
                                    <v-list-item-subtitle>{{formData.fromDate}}</v-list-item-subtitle>
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-col>
                    <v-col cols="12" md="6" >
                        <v-list three-line subheader class="ma-0 pa-0">
                            <v-list-item >
                                <v-list-item-content>
                                    <v-list-item-title>To</v-list-item-title>
                                    <v-list-item-subtitle>{{formData.toDate}}</v-list-item-subtitle>
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-col>
                    <v-col cols="12" md="6" >
                        <v-list three-line subheader class="ma-0 pa-0">
                            <v-list-item >
                                <v-list-item-content>
                                    <v-list-item-title>Employee Name</v-list-item-title>
                                    <v-list-item-subtitle>{{formData.employeeName}}</v-list-item-subtitle>
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-col>
                    <v-col cols="12" md="6" >
                        <v-list three-line subheader class="ma-0 pa-0">
                            <v-list-item >
                                <v-list-item-content>
                                    <v-list-item-title>Approved By</v-list-item-title>
                                    <v-list-item-subtitle>{{formData.approvedByEmployee}}</v-list-item-subtitle>
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-col>
                    <v-col cols="12" md="6" >
                        <v-list three-line subheader class="ma-0 pa-0">
                            <v-list-item >
                                <v-list-item-content>
                                    <v-list-item-title>Replacement Employee</v-list-item-title>
                                    <v-list-item-subtitle>{{formData.replacementEmployeeName}}</v-list-item-subtitle>
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-col>
                    <v-col cols="12" md="6" >
                        <v-list three-line subheader class="ma-0 pa-0">
                            <v-list-item >
                                <v-list-item-content>
                                    <v-list-item-title>Leave Remaining</v-list-item-title>
                                    <v-list-item-subtitle>{{formData.remainingLeave}}</v-list-item-subtitle>
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-col>
                    <v-col cols="12">
                         <v-list three-line subheader class="ma-0 pa-0">
                            <v-list-item >
                                <v-list-item-content>
                                    <v-list-item-title>Remarks</v-list-item-title>
                                    <v-list-item-subtitle>{{formData.Remarks}}</v-list-item-subtitle>
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-col>
                </v-row>
            </v-container>
        </v-card-text>

        <v-card-actions v-if="formData.status ==='Pending'">
            <v-spacer></v-spacer>
            <v-btn class="red" dark text @click="saveData('Rejected')">Reject</v-btn>
            <v-btn class="teal darken-1" dark text @click="saveData('Approved')">Accept</v-btn>
        </v-card-actions>
    </v-card>
</template>

<script>
    import axios from 'axios'

    export default {
        name: "LeaveApplicationEdit",
        props: {
            getItemUrl: {
                type: String,
                required: true
            }
        },
        computed: {
            ItemDetailUrl() {
                return this.getItemUrl
            },
        },
        data() {
            return {
                snackbar: {
                    data: {},
                    message: '',
                    color: '',
                    dateMenu1: null,
                    dateMenu2: null,
                },
                formData: {},
                sectionList: [],
                approvedByList: [],
                employeeList: [],
                leaveTypeList: [],
                replacementEmployeeList: [],
                leaveDaysList: ['Full Day', 'Half Day'],
                departmentList: [],
                designationList: [],
            }
        },
        methods: {
            closeForm() {
                if(this.snackbar.color === 'error'){
                    this.snackbar.message = ''
                }
                this.$emit("closeDialog", this.snackbar)
                this.$v.$reset()
            },
            async saveData(status) {
                try {
                    const {leaveApplicationID} = this.formData
                    let params = {
                        leaveApplicationID,
                        status
                    }
                    const {data} = await axios.post('LeaveApplication/UpdatePendingLeaveApplication', params)
                    this.snackbar.message = data.success ? 'Application Updated Successfully' : data.errors.join(', ')
                    this.snackbar.color = data.success ? 'success' : 'error'
                    this.snackbar.data = data
                    if (data.success) {
                        this.closeForm()
                    } else {
                        this.snackbar.snackbar = true
                    }
                } catch (e) {
                    this.snackbar.message = e
                    this.snackbar.color = 'error'
                    this.snackbar.data = e
                    this.snackbar.snackbar = true
                }
            },
            async getEditData() {
                try {
                    const {data} = await axios.get(this.ItemDetailUrl)
                    this.formData = data
                    this.formData.fromDate = this.$moment(this.formData.fromDate).format('YYYY-MM-DD')
                    this.formData.toDate = this.$moment(this.formData.toDate).format('YYYY-MM-DD')
                    //await this.getSectionList(this.formData.departmentID)
                } catch (e) {
                    console.log(e)
                }

            },
            async getReplacementList() {
                const {data} = await axios.get('Employee/DDLEmployeeList')
                this.replacementEmployeeList = data
            },
            async getApprovedByList() {
                const {data} = await axios.get('Employee/DDLEmployeeList')
                this.approvedByList = data
            },
            async getUserList() {
                const {data} = await axios.get('Employee/DDLEmployeeList')
                this.employeeList = data
            },
            async getLeaveTypeList() {
                let {data} = await axios.get('LeaveSetup/DDLLeaveTypeList')
                this.leaveTypeList = data
            },
            async getDepartmentList() {
                let {data} = await axios.get('Department/DDLDepartmentList')
                this.departmentList = data
            },
            async getSectionList(id) {
                const {data} = await axios.get('Section/DDLSectionList/' + id)
                this.sectionList = data
            },
            async getDesignationList() {
                const {data} = await axios.get('Designation/DDLDesignationList')
                this.designationList = data
            },
            getFormData() {
                this.getReplacementList()
                this.getLeaveTypeList()
                this.getApprovedByList()
                this.getUserList()
                this.getDepartmentList()
                this.getDesignationList()
            },
        },
        created() {
            //this.getFormData()
            this.getEditData()
        },
    }
</script>

<style scoped lang="scss">
    $grey1: #eeeeee99;
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
</style>