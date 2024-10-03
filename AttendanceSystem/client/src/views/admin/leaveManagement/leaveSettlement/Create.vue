<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Leave Settlement</span>
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
                    <v-col cols="12" sm="8">
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
                    <v-col cols="12" md="4">
                        <v-autocomplete v-model="formData.leaveID"
                                        :items="leaveTypeList"
                                        item-value="id"
                                        item-text="value"
                                        label="Leave Type"
                                        :error-messages="leaveIDErrors"
                                        @input="$v.formData.leaveID.$touch()"
                                        @blur="$v.formData.leaveID.$touch()"
                                        clearable
                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12" sm="4">
                        <v-text-field label="Opening Balance"
                                      :value="formData.openingBalance"
                                      type="number"
                                      disabled
                                      :error-messages="openingBalanceErrors"
                                      @input="$v.formData.openingBalance.$touch()"
                                      @blur="$v.formData.openingBalance.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="4">
                        <v-text-field label="Leave Taken"
                                      :value="formData.leaveTaken"
                                      type="number"
                                      disabled
                                      :error-messages="leaveTakenErrors"
                                      @input="$v.formData.leaveTaken.$touch()"
                                      @blur="$v.formData.leaveTaken.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="4">
                        <v-text-field label="Remaining Leave"
                                      :value="formData.remainingLeave"
                                      type="number"
                                      disabled
                                      :error-messages="remainingLeaveErrors"
                                      @input="$v.formData.remainingLeave.$touch()"
                                      @blur="$v.formData.remainingLeave.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="4">
                        <v-text-field label="Carry To Next"
                                      v-model.number="formData.carryToNext"
                                      type="number"
                                      min="0"
                                      :disabled="!carryAbleLeave"
                                      :error-messages="carryToNextErrors"
                                      @input="$v.formData.carryToNext.$touch()"
                                      @blur="$v.formData.carryToNext.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="4">
                        <v-text-field label="Pay"
                                      v-model.number="formData.pay"
                                      type="number"
                                      min="0"
                                      :disabled="!carryAbleLeave"
                                      :error-messages="payErrors"
                                      @input="$v.formData.pay.$touch()"
                                      @blur="$v.formData.pay.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="4">
                        <v-text-field label="Settling Leave"
                                      :value="formData.settlingLeave"
                                      disabled
                                      type="number"
                        ></v-text-field>
                    </v-col>
                </v-row>
            </v-container>
        </v-card-text>

        <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="red darken-1" text @click="closeForm">Cancel</v-btn>
            <v-btn color="teal darken-1" text @click="saveData">Save</v-btn>
        </v-card-actions>
    </v-card>
</template>

<script>
    import {required} from "vuelidate/lib/validators";
    import axios from 'axios'

    export default {
        name: "LeaveSettlementCreate",
        computed: {
            carryAbleLeave(){
                return this.formData.isLeaveCarryable?this.formData.isLeaveCarryable:false
            },
            openingBalanceErrors() {
                const errors = []
                if (!this.$v.formData.openingBalance.$dirty) return errors
                !this.$v.formData.openingBalance.required && errors.push('Opening Balance is required.')
                return errors
            },
            leaveTakenErrors() {
                const errors = []
                if (!this.$v.formData.leaveTaken.$dirty) return errors
                !this.$v.formData.leaveTaken.required && errors.push('Input is required.')
                return errors
            },
            carryToNextErrors() {
                const errors = []
                if (!this.$v.formData.carryToNext.$dirty) return errors
                !this.$v.formData.carryToNext.required && errors.push('Input is required.')
                return errors
            },
            payErrors() {
                const errors = []
                if (!this.$v.formData.pay.$dirty) return errors
                !this.$v.formData.pay.required && errors.push('Pay is required.')
                return errors
            },
            employeeIDErrors() {
                const errors = []
                if (!this.$v.formData.employeeID.$dirty) return errors
                !this.$v.formData.employeeID.required && errors.push('Employee Name is required.')
                return errors
            },
            leaveIDErrors() {
                const errors = []
                if (!this.$v.formData.leaveID.$dirty) return errors
                !this.$v.formData.leaveID.required && errors.push('Leave Type is required.')
                return errors
            },
            remainingLeaveErrors() {
                const errors = []
                if (!this.$v.formData.remainingLeave.$dirty) return errors
                !this.$v.formData.remainingLeave.required && errors.push('Remaining Leave is required.')
                return errors
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
                formData: {
                    carryToNext: 0,
                    pay: 0,
                    leaveTaken: 0,
                },
                sectionList: [],
                settlingLeaveList: [],
                employeeList: [],
                leaveTypeList: [],
                replacementEmployeeList: [],
                leaveDaysList: [],
                departmentList: []
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
            async getUserList() {
                const {data} = await axios.get('Employee/DDLEmployeeList')
                this.employeeList = data
            },
            async getLeaveTypeList() {
                let {data} = await axios.get('LeaveSetup/DDLGenderWiseLeaveTypeList/' + this.formData.employeeID)
                this.leaveTypeList = data
            },
            async getLeaveSettlementData() {
                let {employeeID, leaveID} = this.formData
                let {data} = await axios.post('LeaveSettlement/GetEmployeeWiseLeaveSettlement', {
                    employeeID, leaveID
                })
                const {openingBalance, leaveTaken, remainingLeave, isLeaveCarryable} = data
                this.formData.openingBalance = openingBalance
                this.formData.leaveTaken = leaveTaken
                this.formData.remainingLeave = remainingLeave
                this.$set(this.formData,'isLeaveCarryable',isLeaveCarryable)
            },
            getFormData() {
                this.getUserList()
            },
            async saveData() {
                this.$v.$touch()
                if (this.$v.$invalid) {
                    this.submitStatus = 'ERROR'
                } else {
                    try {
                        const {data} = await axios.post('LeaveSettlement/CreateLeaveSettlement', this.formData)
                        this.snackbar.message = data.success ? 'Leave Created Successfully' : data.errors.join(', ')
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
            calculateSettlingLeave() {
                this.formData.settlingLeave = this.formData.openingBalance - this.formData.leaveTaken - this.formData.carryToNext - this.formData.pay

            }
        },
        created() {
            this.getFormData()
        },
        validations: {
            formData: {
                openingBalance: {required},
                leaveTaken: {required},
                carryToNext: {required},
                pay: {required},
                employeeID: {required},
                leaveID: {required},
                remainingLeave: {required},
            }
        },
        watch: {
            'formData.employeeID': {
                handler(value) {
                    if(value) this.getLeaveTypeList()
                },
                deep: true,
                immediate:false
            },
            'formData.leaveID': {
                handler(value) {
                    if(value) this.getLeaveSettlementData()
                },
                deep: true,
                immediate:false
            },
            'formData.openingBalance': {
                handler(value) {
                    if(value) this.calculateSettlingLeave()
                },
                deep: true,
                immediate:false
            },
            'formData.carryToNext': {
                handler(value) {
                   if(value>=0) this.calculateSettlingLeave()
                },
                deep: true,
                immediate:false
            },
            'formData.pay': {
                handler(value) {
                   if(value>=0) this.calculateSettlingLeave()
                },
                deep: true,
                immediate:false
            },
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