<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline"> Office Visit</span>
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
                        <div class="d-flex flex-row">
                            <v-menu
                                    ref="dateMenu1"
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
                                            label="From"
                                            readonly
                                            prepend-icon="event"
                                            v-bind="attrs"
                                            v-on="on"
                                            :error-messages="fromDateErrors"
                                            @input="$v.formData.fromDate.$touch()"
                                            @blur="$v.formData.fromDate.$touch()"
                                    ></v-text-field>
                                </template>
                                <v-date-picker v-model="formData.fromDate" no-title
                                               :max="maxFromDate"
                                               :min="minFromDate"
                                               @input="snackbar.dateMenu1 = false"></v-date-picker>
                            </v-menu>
                            <nepaliDatePicker
                                    v-else
                                    refer="date"
                                    v-model="formData.fromDate"
                                    :max="maxFromDate"
                                    :min="minFromDate"
                                    :error-messages="fromDateErrors"
                                    @input="$v.formData.fromDate.$touch()"
                                    @blur="$v.formData.fromDate.$touch()"
                            ></nepaliDatePicker>
                            <v-digital-time-picker
                                    v-model="formData.fromTime"
                                    prepend-icon="access_time"
                                    :error-messages="fromTimeErrors"
                                    @input="$v.formData.fromTime.$touch()"
                                    @blur="$v.formData.fromTime.$touch()"
                            />
                        </div>
                    </v-col>
                    <v-col cols="12" md="6">
                        <div class="d-flex flex-row">
                            <v-menu
                                    ref="dateMenu2"
                                    v-model="snackbar.dateMenu2"
                                    :close-on-content-click="false"
                                    transition="scale-transition"
                                    offset-y
                                    min-width="290px"
                                    v-if="!checkDateLang"
                            >
                                <template v-slot:activator="{ on, attrs }">
                                    <v-text-field
                                            v-model="formData.fromDate"
                                            label="To"
                                            readonly
                                            prepend-icon="event"
                                            v-bind="attrs"
                                            v-on="on"
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
                                    v-model="formData.toDate"
                                    :min="minToDate"
                                    :error-messages="toDateErrors"
                                    @input="$v.formData.toDate.$touch()"
                                    @blur="$v.formData.toDate.$touch()"
                            ></nepaliDatePicker>
                            <v-digital-time-picker
                                    v-model="formData.toTime"
                                    prepend-icon="access_time"
                                    :error-messages="toTimeErrors"
                                    @input="$v.formData.toTime.$touch()"
                                    @blur="$v.formData.toTime.$touch()"
                            />
                        </div>
                    </v-col>
                    <v-col cols="12" md="12">
                        <v-autocomplete v-model="formData.employeeID"
                                        :items="employeeList"
                                        item-value="id"
                                        item-text="value"
                                        :error-messages="employeeIDErrors"
                                        @input="$v.formData.employeeID.$touch()"
                                        @blur="$v.formData.employeeID.$touch()"
                                        label="Employee"
                                        clearable

                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="12">
                        <v-text-field label="Visitors Name"
                                      v-model="formData.visitorName"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12">
                        <v-textarea
                                outlined
                                name="input-7-4"
                                label="Remarks"
                                v-model="formData.remarks"
                        ></v-textarea>
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
    import nepaliDatePicker from "../../../../components/nepaliDatePicker";
    import { mapGetters } from 'vuex'

    export default {
        name: "OfficeVisitCreate",
        components:{
            nepaliDatePicker
        },
        computed: {
            officeVisitErrors() {
                const errors = []
                if (!this.$v.formData.officeVisit.$dirty) return errors
                !this.$v.formData.officeVisit.required && errors.push('Office Visit is required.')
                return errors
            },
            fromDateErrors() {
                const errors = []
                if (!this.$v.formData.fromDate.$dirty) return errors
                !this.$v.formData.fromDate.required && errors.push('Date is required.')
                return errors
            },
            fromTimeErrors() {
                const errors = []
                if (!this.$v.formData.fromTime.$dirty) return errors
                !this.$v.formData.fromTime.required && errors.push('Time is required.')
                return errors
            },
            toDateErrors() {
                const errors = []
                if (!this.$v.formData.toDate.$dirty) return errors
                !this.$v.formData.toDate.required && errors.push('Date is required.')
                return errors
            },
            toTimeErrors() {
                const errors = []
                if (!this.$v.formData.toTime.$dirty) return errors
                !this.$v.formData.toTime.required && errors.push('Time is required.')
                return errors
            },
            employeeIDErrors() {
                const errors = []
                if (!this.$v.formData.employeeID.$dirty) return errors
                !this.$v.formData.employeeID.required && errors.push('Employee is required.')
                return errors
            },
            minToDate() {
                return this.formData.fromDate || this.$moment().format('YYYY-MM-DD')
            },
            maxFromDate() {
                return this.formData.toDate
            },
            minFromDate() {
                return this.$moment().format('YYYY-MM-DD')
            },
            ...mapGetters(['checkDateLang'])
        },
        data() {
            return {
                snackbar: {
                    data: {},
                    message: '',
                    color: '',
                    snackbar: false
                },
                dateMenu1: false,
                dateMenu2: false,
                timeMenu1: false,
                timeMenu2: false,
                formData: {},
                employeeList: [],
                submitStatus: ''
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
            getFormData() {
                this.getUserList()
            },
            async saveData() {
                this.$v.$touch()
                if (this.$v.$invalid) {
                    this.submitStatus = 'ERROR'
                } else {
                    try {
                        const {data} = await axios.post('OfficeVisit/CreateOfficeVisit', this.formData)
                        this.snackbar.message = data.success ? 'Data Created Successfully' : data.errors.join(', ')
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

            }
        },
        created() {
            this.getFormData()
        },
        validations: {
            formData: {
                employeeID: {required},
                fromDate: {required},
                fromTime: {required},
                toDate: {required},
                toTime: {required},
            }
        },
    }
</script>

<style scoped>

</style>