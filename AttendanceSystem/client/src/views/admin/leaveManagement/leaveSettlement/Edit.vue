<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Edit Settlement</span>
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
                    <v-col cols="12" md="8">
                        <v-row no-gutters>
                            <v-col cols="12">
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
                            <v-col cols="12" sm="6" class="px-1">
                                <v-text-field label="Opening Balance"
                                              v-model.number="formData.openingBalance"
                                              type="number"
                                              :error-messages="openingBalanceErrors"
                                              @input="$v.formData.openingBalance.$touch()"
                                              @blur="$v.formData.openingBalance.$touch()"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" sm="6" class="px-1">
                                <v-text-field label="Leave Taken"
                                              v-model.number="formData.leaveTaken"
                                              type="number"
                                              :error-messages="leaveTakenErrors"
                                              @input="$v.formData.leaveTaken.$touch()"
                                              @blur="$v.formData.leaveTaken.$touch()"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" sm="6" class="px-1">
                                <v-text-field label="Carry To Next"
                                              v-model.number="formData.carryToNext"
                                              type="number"
                                              :error-messages="carryToNextErrors"
                                              @input="$v.formData.carryToNext.$touch()"
                                              @blur="$v.formData.carryToNext.$touch()"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" sm="6" class="px-1">
                                <v-text-field label="Pay"
                                              v-model.number="formData.pay"
                                              type="number"
                                              :error-messages="payErrors"
                                              @input="$v.formData.pay.$touch()"
                                              @blur="$v.formData.pay.$touch()"
                                ></v-text-field>
                            </v-col>
                        </v-row>
                    </v-col>
                    <v-col cols="12" md="4">
                        <v-row no-gutters>
                            <v-col cols="12">
                                <v-autocomplete v-model="formData.leaveType"
                                                :items="leaveTypeList"
                                                item-value="id"
                                                item-text="value"
                                                label="Leave Type"
                                                :error-messages="leaveTypeErrors"
                                                @input="$v.formData.leaveType.$touch()"
                                                @blur="$v.formData.leaveType.$touch()"
                                                clearable
                                ></v-autocomplete>
                            </v-col>
                            <v-col cols="12">
                                <v-text-field label="Remaining Leave"
                                              v-model.number="formData.remainingLeave"
                                              type="number"
                                              :error-messages="remainingLeaveErrors"
                                              @input="$v.formData.remainingLeave.$touch()"
                                              @blur="$v.formData.remainingLeave.$touch()"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12">
                                <v-text-field label="Settling Leave"
                                              v-model.number="formData.settlingLeave"
                                              type="number"
                                              :error-messages="settlingLeaveErrors"
                                              @input="$v.formData.settlingLeave.$touch()"
                                              @blur="$v.formData.settlingLeave.$touch()"
                                ></v-text-field>
                            </v-col>
                        </v-row>
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
    import axios from 'axios'
    import { required } from "vuelidate/lib/validators"

    export default {
        name: "LeaveSettlementEdit",
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
            openingBalanceErrors() {
                const errors = []
                if (!this.$v.formData.openingBalance.$dirty) return errors
                !this.$v.formData.openingBalance.required && errors.push('Code is required.')
                return errors
            },
            leaveTakenErrors() {
                const errors = []
                if (!this.$v.formData.leaveTaken.$dirty) return errors
                !this.$v.formData.leaveTaken.required && errors.push('Designation is required.')
                return errors
            },
            carryToNextErrors() {
                const errors = []
                if (!this.$v.formData.carryToNext.$dirty) return errors
                !this.$v.formData.carryToNext.required && errors.push('Department is required.')
                return errors
            },
            payErrors() {
                const errors = []
                if (!this.$v.formData.pay.$dirty) return errors
                !this.$v.formData.pay.required && errors.push('Section is required.')
                return errors
            },
            employeeIDErrors() {
                const errors = []
                if (!this.$v.formData.employeeID.$dirty) return errors
                !this.$v.formData.employeeID.required && errors.push('Employee Name is required.')
                return errors
            },
            leaveTypeErrors() {
                const errors = []
                if (!this.$v.formData.leaveType.$dirty) return errors
                !this.$v.formData.leaveType.required && errors.push('Date is required.')
                return errors
            },
            remainingLeaveErrors() {
                const errors = []
                if (!this.$v.formData.remainingLeave.$dirty) return errors
                !this.$v.formData.remainingLeave.required && errors.push('Date is required.')
                return errors
            },
            settlingLeaveErrors() {
                const errors = []
                if (!this.$v.formData.settlingLeave.$dirty) return errors
                !this.$v.formData.settlingLeave.required && errors.push('Date is required.')
                return errors
            },
            leaveDaysErrors() {
                const errors = []
                if (!this.$v.formData.leaveDay.$dirty) return errors
                !this.$v.formData.leaveDay.required && errors.push('Input is Required')
                return errors
            },
        },
        data() {
            return {
                snackbar: {
                    data: {},
                    message: '',
                    color: '',
                    dateMenu1:null,
                    dateMenu2:null,
                },
                formData: {},
                sectionList: [],
                approvedByList: [],
                employeeList: [],
                leaveTypeList: [],
                replacementEmployeeList: [],
                leaveDaysList: [],
                departmentList: []
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
            async saveData() {
                this.$v.$touch()
                this.$v.$touch()
                if (this.$v.$invalid) {
                    this.submitStatus = 'ERROR'
                } else {
                    try {
                        const {data} = await axios.post('LeaveSettlement/UpdateLeaveSettlement', this.formData)
                        this.snackbar.message = data.success ? 'Settlement Updated Successfully' : data.errors.join(', ')
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
            async getEditData() {
                try {
                    const {data} = await axios.get(this.ItemDetailUrl)
                    this.formData = data
                } catch (e) {
                    console.log(e)
                }

            },
            async getUserList() {
                const {data} = await axios.get('Employee/DDLEmployeeList')
                this.employeeList = data
            },
            async getLeaveTypeList() {
                let {data} = await axios.get('LeaveSettlement/DDLLeaveTypeList')
                this.leaveTypeList = data
            },
            getFormData() {
                this.getLeaveTypeList()
                this.getUserList()
            },
        },
        created() {
            this.getFormData()
            this.getEditData()
        },
        validations: {
            formData: {
                openingBalance: {required},
                leaveTaken: {required},
                carryToNext: {required},
                pay: {required},
                employeeID: {required},
                leaveType: {required},
                remainingLeave: {required},
                settlingLeave: {required},
            }
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