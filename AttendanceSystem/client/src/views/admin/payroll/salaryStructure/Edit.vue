<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Edit Allowance</span>
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
                    <v-col cols="12">
                        <v-text-field label="Designation Level"
                                      v-model="formData.designationLevel"
                                      :error-messages="designationLevelErrors"
                                      @input="$v.formData.designationLevel.$touch()"
                                      @blur="$v.formData.designationLevel.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12">
                        <v-text-field label="Designation Name"
                                      v-model="formData.designationName"
                                      :error-messages="designationNameErrors"
                                      @input="$v.formData.designationName.$touch()"
                                      @blur="$v.formData.designationName.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12">
                        <v-text-field label="Basic Salary"
                                      v-model.number="formData.basicSalary"
                                      type="number"
                                      :error-messages="basicSalaryErrors"
                                      @input="$v.formData.basicSalary.$touch()"
                                      @blur="$v.formData.basicSalary.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12">
                        <v-text-field label="OT Rate (Per Hour)"
                                      v-model.number="formData.otRatePerHour"
                                      type="number"
                                      :error-messages="otRatePerHourErrors"
                                      @input="$v.formData.otRatePerHour.$touch()"
                                      @blur="$v.formData.otRatePerHour.$touch()"
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
    import axios from 'axios'
    import {required} from "vuelidate/lib/validators"

    export default {
        name: "AllowancesEdit",
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
            otRatePerHourErrors() {
                const errors = []
                if (!this.$v.formData.otRatePerHour.$dirty) return errors
                !this.$v.formData.otRatePerHour.required && errors.push('OT Rate (Per Hour) is required.')
                return errors
            },
            basicSalaryErrors() {
                const errors = []
                if (!this.$v.formData.basicSalary.$dirty) return errors
                !this.$v.formData.basicSalary.required && errors.push('Basic Salary is required.')
                return errors
            },
            designationLevelErrors() {
                const errors = []
                if (!this.$v.formData.designationLevel.$dirty) return errors
                !this.$v.formData.designationLevel.required && errors.push('Designation Level is required.')
                return errors
            },
            designationNameErrors() {
                const errors = []
                if (!this.$v.formData.designationName.$dirty) return errors
                !this.$v.formData.designationName.required && errors.push('Allowance is required.')
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
                    snackbar:false
                },
                formData: {},
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
                        const {data} = await axios.post('Allowances/UpdateAllowances', this.formData)
                        this.snackbar.message = data.success ? 'Allowance Updated Successfully' : data.errors.join(', ')
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
            getFormData() {

            },
        },
        created() {
            this.getEditData()
        },
        validations: {
            formData: {
                basicSalary: {required},
                otRatePerHour: {required},
                designationLevel: {required},
                designationName: {required},
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