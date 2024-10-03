<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Add Application</span>
        </v-card-title>

        <v-card-text>
            <v-snackbar :color="snackbar.color"
                        top
                        v-model="snackbar.snackbar">
                {{ snackbar.message }}
                <v-btn @click="snackbar.snackbar = false"
                       dark
                       text>
                    Close
                </v-btn>
            </v-snackbar>
            <v-container>
                <v-row>
                    <v-col cols="12" md="6">
                        <v-text-field label="Code"
                                      v-model="formData.code"
                                      :error-messages="codeErrors"
                                      @input="$v.formData.code.$touch()"
                                      @blur="$v.formData.code.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-radio-group v-model="formData.replacementLeaveType"
                                       row
                                       @change="formDataReset"
                        >
                            <v-radio
                                    v-for="(item,value) in leaveCategory"
                                    :key="'CheckClockIN'+value"
                                    :label="item.value" :value="item.id" class="pt-1"></v-radio>
                        </v-radio-group>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-autocomplete v-model="formData.leaveID"
                                        :items="leaveTypeList"
                                        item-value="id"
                                        item-text="value"
                                        label="Leave Type"
                                        clearable
                                        v-if="!hasReplacementLeaveType"
                                        :error-messages="leaveIDErrors"
                                        @input="$v.formData.leaveID.$touch()"
                                        @blur="$v.formData.leaveID.$touch()"
                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-autocomplete v-model="formData.leaveDay"
                                        :items="leaveDaysList"
                                        label="Leave Day"
                                        clearable
                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-autocomplete v-model="formData.departmentID"
                                        :items="departmentList"
                                        :error-messages="departmentIDErrors"
                                        @input="$v.formData.departmentID.$touch()"
                                        @blur="$v.formData.departmentID.$touch()"
                                        @change="getSectionList"
                                        label="Department"
                                        item-value="id"
                                        item-text="value"
                                        clearable
                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-autocomplete v-model="formData.sectionID"
                                        :items="sectionList"
                                        :error-messages="sectionIDErrors"
                                        @input="$v.formData.sectionID.$touch()"
                                        @blur="$v.formData.sectionID.$touch()"
                                        label="Section"
                                        item-value="id"
                                        item-text="value"
                                        clearable
                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-autocomplete v-model="formData.designationID"
                                        :items="designationList"
                                        :error-messages="designationIDErrors"
                                        @input="$v.formData.designationID.$touch()"
                                        @blur="$v.formData.designationID.$touch()"
                                        item-value="id"
                                        item-text="value"
                                        label="Designation"
                                        clearable
                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-autocomplete v-model="formData.employeeID"
                                        :items="employeeList"
                                        :error-messages="employeeIDErrors"
                                        @input="$v.formData.employeeID.$touch()"
                                        @blur="$v.formData.employeeID.$touch()"
                                        label="Employee Name"
                                        item-value="id"
                                        item-text="value"
                                        clearable
                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="6" v-if="!isReplacementLeave">
                        <v-text-field label="Remaining Leave"
                                      v-model.number="formData.remainingLeave"
                                      type="number"
                                      readonly
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6" v-if="formData.employeeID">
                        <v-menu
                                ref="menu1"
                                v-model="snackbar.dateMenu1"
                                :close-on-content-click="false"
                                transition="scale-transition"
                                offset-y
                                min-width="290px"
                                v-if="!checkDateLang"
                        >
                            <template v-slot:activator="{ on, attrs }">
                                <v-text-field
                                        v-model="formData.fromDate"
                                        label="From Date"
                                        readonly
                                        v-bind="attrs"
                                        v-on="on"
                                        clearable
                                        :disabled="haltOnNoLeaveRemaining"
                                        :error-messages="fromDateErrors"
                                        @input="$v.formData.fromDate.$touch()"
                                        @blur="$v.formData.fromDate.$touch()"
                                ></v-text-field>
                            </template>
                            <v-date-picker v-model="formData.fromDate" no-title
                                           :max="maxFromDate"
                                           @input="snackbar.dateMenu1 = false"></v-date-picker>
                        </v-menu>
                        <nepaliDatePicker
                                v-else
                                refer="date"
                                label="From Date"
                                v-model="formData.fromDate"
                                :max="maxFromDate"
                                :disabled="haltOnNoLeaveRemaining"
                                :error-messages="fromDateErrors"
                                @input="$v.formData.fromDate.$touch()"
                                @blur="$v.formData.fromDate.$touch()"
                        ></nepaliDatePicker>
                    </v-col>
                    <v-col cols="12" md="6" v-if="formData.employeeID">
                        <v-menu
                                ref="menu1"
                                v-model="snackbar.dateMenu2"
                                :close-on-content-click="false"
                                transition="scale-transition"
                                offset-y
                                min-width="290px"
                                v-if="!checkDateLang"
                        >
                            <template v-slot:activator="{ on, attrs }">
                                <v-text-field
                                        v-model="formData.toDate"
                                        label="To Date"
                                        readonly
                                        v-bind="attrs"
                                        v-on="on"
                                        clearable
                                        :disabled="haltOnNoLeaveRemaining"
                                        :error-messages="toDateErrors"
                                        @input="$v.formData.toDate.$touch()"
                                        @blur="$v.formData.toDate.$touch()"
                                ></v-text-field>
                            </template>
                            <v-date-picker v-model="formData.toDate" no-title
                                           :min="minToDate"
                                           @input="snackbar.dateMenu2 = false"></v-date-picker>
                        </v-menu>
                        <nepaliDatePicker
                                v-else
                                refer="date"
                                label="To Date"
                                :disabled="haltOnNoLeaveRemaining"
                                v-model="formData.toDate"
                                :error-messages="toDateErrors"
                                :min="minToDate"
                                @input="$v.formData.toDate.$touch()"
                                @blur="$v.formData.toDate.$touch()"
                        ></nepaliDatePicker>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-autocomplete v-model="formData.approvedBy"
                                        :items="approvedByList"
                                        label="Approved By"
                                        :error-messages="approvedByErrors"
                                        @input="$v.formData.approvedBy.$touch()"
                                        @blur="$v.formData.approvedBy.$touch()"
                                        item-value="id"
                                        item-text="value"
                                        clearable
                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-textarea
                                outlined
                                name="input-7-4"
                                label="Remarks"
                                v-model="formData.remarks"
                        ></v-textarea>
                    </v-col>
                    <v-col cols="12" md=6 v-if="isReplacementLeave">
                        <v-autocomplete v-model="formData.replacementEmployeeId"
                                        :items="replacementEmployeeList"
                                        label="Replacement Employee"
                                        item-value="employeeID"
                                        item-text="shiftDetails.employeeName"
                                        clearable
                        ></v-autocomplete>
                    </v-col>
                </v-row>
            </v-container>
        </v-card-text>

        <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="red darken-1" text @click="closeForm">Cancel</v-btn>
            <v-btn color="teal darken-1" text @click="saveData" :disabled="haltOnNoLeaveRemaining">Save</v-btn>
        </v-card-actions>
    </v-card>
</template>

<script>
    import {required, requiredUnless} from "vuelidate/lib/validators";
    import axios from 'axios'
    import nepaliDatePicker from "../../../../components/nepaliDatePicker";
import { mapGetters } from 'vuex'

    const difference = (dateDifference, remainingLeave, notReplacementLeave) =>
        () => notReplacementLeave ? dateDifference < remainingLeave : true
    export default {
        name: "LeaveApplicationCreate",
        components:{
            nepaliDatePicker
        },
        computed: {
            hasReplacementLeaveType() {
                return this.formData.replacementLeaveType !== 'none' && this.formData.replacementLeaveType
            },
            isReplacementLeave() {
                return this.formData.replacementLeaveType === 'Replacement'
            },
            isOTLeave() {
                return this.formData.replacementLeaveType === 'OT'
            },
            currentBS2ADDate(){
                let nepaliDateObject = window.NepaliFunctions.GetCurrentBsDate()
                nepaliDateObject.day = 1
                let englishDateObject = window.NepaliFunctions.BS2AD(nepaliDateObject)
                let returns = window.NepaliFunctions.ConvertDateFormat(englishDateObject, "YYYY-MM-DD")
                return returns
            },
            dateDifference() {
                return Math.abs(this.$moment(this.formData.fromDate).diff(this.$moment(this.formData.toDate), 'days')) || 0
            },
            codeErrors() {
                const errors = []
                if (!this.$v.formData.code.$dirty) return errors
                !this.$v.formData.code.required && errors.push('Code is required.')
                return errors
            },
            designationIDErrors() {
                const errors = []
                if (!this.$v.formData.designationID.$dirty) return errors
                !this.$v.formData.designationID.required && errors.push('Designation is required.')
                return errors
            },
            departmentIDErrors() {
                const errors = []
                if (!this.$v.formData.departmentID.$dirty) return errors
                !this.$v.formData.departmentID.required && errors.push('Department is required.')
                return errors
            },
            sectionIDErrors() {
                const errors = []
                if (!this.$v.formData.sectionID.$dirty) return errors
                !this.$v.formData.sectionID.required && errors.push('Section is required.')
                return errors
            },
            employeeIDErrors() {
                const errors = []
                if (!this.$v.formData.employeeID.$dirty) return errors
                !this.$v.formData.employeeID.required && errors.push('Employee Name is required.')
                return errors
            },
            fromDateErrors() {
                const errors = []
                if (!this.$v.formData.fromDate.$dirty) return errors
                !this.$v.formData.fromDate.required && errors.push('Date is required.')
                !this.$v.formData.fromDate.difference && errors.push('Remaining Leave exceeds.Please Date wisely according to remaining leave')
                return errors
            },
            toDateErrors() {
                const errors = []
                if (!this.$v.formData.toDate.$dirty) return errors
                !this.$v.formData.toDate.required && errors.push('Date is required.')
                !this.$v.formData.toDate.difference && errors.push('Remaining Leave exceeds.Please Date wisely according to remaining leave')
                return errors
            },
            approvedByErrors() {
                const errors = []
                if (!this.$v.formData.approvedBy.$dirty) return errors
                !this.$v.formData.approvedBy.required && errors.push('Field is required.')
                return errors
            },
            leaveDaysErrors() {
                const errors = []
                if (!this.$v.formData.leaveDay.$dirty) return errors
                !this.$v.formData.leaveDay.required && errors.push('Input is Required')
                return errors
            },
            leaveIDErrors() {
                const errors = []
                if (!this.$v.formData.leaveID.$dirty) return errors
                !this.$v.formData.leaveID.required && errors.push('Input is Required')
                return errors
            },
            isOptional() {
                return this.isOTLeave || this.isReplacementLeave // some conditional logic here...
            },
            minToDate(){
               return this.formData.fromDate
            },
            maxFromDate(){
                return this.formData.toDate
            },
            haltOnNoLeaveRemaining(){
                return !this.isReplacementLeave && !this.formData.remainingLeave
            },
            ...mapGetters(['checkDateLang'])
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
                formData: {
                    replacementLeaveType: 'none',
                    remainingLeave: null,
                    fromDate: null,
                    toDate: null,
                    designationID:null,
                    leaveID:null
                },
                sectionList: [],
                approvedByList: [],
                employeeList: [],
                leaveTypeList: [],
                replacementEmployeeList: [],
                leaveDaysList: ['Full Day', 'Half Day'],
                leaveCategory: [
                    {id: 'OT', value: 'OT leave'},
                    {id: 'Replacement', value: 'Replacement leave'},
                    {id: 'none', value: 'None'},
                ],
                departmentList: [],
                designationList: [],
            }
        },
        methods: {
            closeForm() {
                this.$v.$reset()
                if(this.snackbar.color === 'error'){
                    this.snackbar.message = ''
                }
                this.$emit("closeDialog", this.snackbar)
            },
            async getReplacementListAndOTData() {
                const {leaveID, employeeID, fromDate, toDate, designationID} = this.formData
                if (this.isReplacementLeave && employeeID && fromDate && toDate) {
                    this.formData.leaveID = null
                    this.formData.remainingLeave = null
                    const {data} = await axios.post('LeaveApplication/GetReplacementEmployeeList',
                        {
                            employeeID, fromDate, toDate, designationID
                        }
                    )
                    this.replacementEmployeeList = data.data
                }

                else if(this.isOTLeave){
                    this.formData.remainingLeave = null
                    this.formData.leaveID = null
                    const {data} = await axios.post('LeaveApplication/GetEmplyeeOT', {
                        employeeID
                    })
                    this.formData.remainingLeave = data.remainingLeave
                    this.replacementEmployeeList = []
                }
                else if (!this.isOTLeave && !this.isReplacementLeave && employeeID && leaveID) {
                    this.formData.remainingLeave = null
                    const {data} = await axios.post('LeaveApplication/GetEmplyeeRemainingLeave', {
                        leaveID,
                        employeeID
                    })
                    this.formData.remainingLeave = data.remainingLeave
                    this.replacementEmployeeList = []
                }
                else {
                    this.replacementEmployeeList = []
                    this.formData.remainingLeave = null
                }
                //await this.getApprovedByList
            },
            /*async getReplacementListAndOTData() {
                const {leaveID, employeeID} = this.formData
                this.formData.remainingLeave = null
                if (this.isOTLeave) {
                    const {data} = await axios.post('LeaveApplication/GetEmplyeeOT', {
                        employeeID
                    })
                    this.formData.remainingLeave = data.availableOTLeave
                }
                if (!this.isOTLeave && !this.isReplacementLeave && this.employeeID && this.leaveID) {
                    const {data} = await axios.post('LeaveApplication/GetEmplyeeRemainingLeave', {
                        leaveID,
                        employeeID
                    })
                    this.formData.remainingLeave = data.remainingLeave
                }
            },*/
            async getApprovedByList() {
                const {data} = await axios.get('Employee/DDLEmployeeList')
                this.approvedByList = data
            },
            async getUserList() {
                const {sectionID, designationID, departmentID} = this.formData
                const {data} = await axios.post('Employee/LeaveApplicantEmployeeList', {
                    sectionID, designationID, departmentID
                })
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
                this.getLeaveTypeList()
                this.getApprovedByList()
                this.getDepartmentList()
                this.getDesignationList()
            },
            async saveData() {
                this.$v.$touch()
                if (this.$v.$invalid) {
                    this.submitStatus = 'ERROR'
                } else {
                    try {
                        this.formData.CurrentStartDate = this.currentBS2ADDate
                        if (this.hasReplacementLeaveType) {
                            this.formData.leaveID = null
                        }
                        const {data} = await axios.post('LeaveApplication/CreateLeaveApplication', this.formData)
                        this.snackbar.message = data.success ? 'Leave Application Created Successfully' : data.errors.join(', ')
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
                }

            },
            formDataReset() {

            },
        },
        created() {
            this.getFormData()
        },
        validations() {
            return {
                formData: {
                    code: {required},
                    leaveID: {
                        required: requiredUnless(function () {
                            return this.isOptional
                        })
                    },
                    designationID: {required},
                    departmentID: {required},
                    sectionID: {required},
                    employeeID: {required},
                    fromDate: {
                        required,
                        difference: difference(this.dateDifference, this.formData.remainingLeave, !this.isReplacementLeave)
                    },
                    toDate: {required,
                        difference:difference(this.dateDifference, this.formData.remainingLeave, !this.isReplacementLeave)
                    },
                    leaveDay: {required},
                    approvedBy: {required},
                }
            }
        },
        watch: {
            'formData.departmentID': {
                handler() {
                    this.getUserList()
                },
                immediate: false
            },
            'formData.sectionID': {
                handler() {
                    this.getUserList()
                },
                immediate: false
            },
            'formData.designationID': {
                handler() {
                    this.getUserList()
                },
                immediate: false
            },
            'formData.toDate': {
                handler() {
                    this.getReplacementListAndOTData()
                },
                immediate: false
            },
            'formData.fromDate': {
                handler() {
                    this.getReplacementListAndOTData()
                },
                immediate: false
            },
            'formData.employeeID': {
                handler() {
                    this.getReplacementListAndOTData()
                },
                immediate: false
            },
            'formData.leaveID': {
                handler() {
                    this.getReplacementListAndOTData()
                },
                immediate: false
            },
            'formData.replacementLeaveType': {
                handler() {
                    this.getReplacementListAndOTData()
                },
                immediate: false
            }
        }
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

    ::v-deep.customFontSize .v-text-field input, ::v-deep.customFontSize .v-input--selection-controls input {
        font-size: 0.8em;
        text-transform: capitalize;
    }

    ::v-deep.customFontSize .v-text-field label, ::v-deep.customFontSize .v-list-item__content .v-list-item__title, ::v-deep.customFontSize .v-input--selection-controls label {
        font-size: 0.8em !important;
    }
</style>