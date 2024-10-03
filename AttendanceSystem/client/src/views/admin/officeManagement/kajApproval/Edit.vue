<template>
    <v-card class="text-left">
        <v-card-title>
            <span class="headline"> Kaj Approval</span>
            <v-spacer></v-spacer>
            <v-btn depressed flat text @click="closeForm">
                <v-icon>mdi-close</v-icon>
            </v-btn>
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
                    <v-col cols="12" md="4">
                        <v-list three-line subheader>
                            <v-list-item >
                                <v-list-item-content>
                                    <v-list-item-title>From (Date and Time)</v-list-item-title>
                                    <v-list-item-subtitle>{{formData.fromDate}} {{' '}} {{formData.fromTimeString}}</v-list-item-subtitle>
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-col>
                    <v-col cols="12" md="4">
                        <v-list three-line subheader>
                            <v-list-item >
                                <v-list-item-content>
                                    <v-list-item-title>To (Date and Time)</v-list-item-title>
                                    <v-list-item-subtitle>{{formData.toDate}} {{' '}} {{formData.toTimeString}}</v-list-item-subtitle>
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-col>
                    <v-col cols="12" md="4">
                        <v-list three-line subheader>
                            <v-list-item >
                                <v-list-item-content>
                                    <v-list-item-title>Employee Name</v-list-item-title>
                                    <v-list-item-subtitle>{{formData.employeeName}}</v-list-item-subtitle>
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-col>

                    <v-col cols="12">
                            <v-list three-line subheader>
                                <v-list-item >
                                    <v-list-item-content>
                                        <v-list-item-title>Remarks</v-list-item-title>
                                        <v-list-item-subtitle>{{formData.remarks}}</v-list-item-subtitle>
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
    import {required} from "vuelidate/lib/validators"

    export default {
        name: "KajEdit",
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
            kajErrors() {
                const errors = []
                if (!this.$v.formData.kaj.$dirty) return errors
                !this.$v.formData.kaj.required && errors.push('Kaj is required.')
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
                if(this.snackbar.color === 'error'){
                    this.snackbar.message = ''
                }
                this.$emit("closeDialog", this.snackbar)
                this.$v.$reset()
            },
            async getUserList() {
                const {data} = await axios.get('Employee/DDLEmployeeList')
                this.employeeList = data
            },
            getFormData() {
                this.getUserList()
            },
            async saveData(status) {
                    try {
                        const { kajID } = this.formData
                        let params = {
                            kajID,
                            status
                        }
                        const {data} = await axios.post('Kaj/UpdatePendingKajAsync', params)
                        this.snackbar.message = data.success ? 'Data Updated Successfully' : data.errors.join(', ')
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
                    this.formData.toTime = data.toTimeString
                    this.formData.fromTime = data.fromTimeString
                } catch (e) {
                    console.log(e)
                }

            }
        },
        created() {
            //this.getFormData()
            this.getEditData()
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