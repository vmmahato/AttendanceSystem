<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Add Designation</span>
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
                        <v-text-field label="Designation"
                                      v-model="formData.designationName"
                                      :error-messages="designationNameErrors"
                                      @input="$v.formData.designationName.$touch()"
                                      @blur="$v.formData.designationName.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12">
                        <v-text-field label="Designation Level"
                                      v-model="formData.designationLevel"
                                      :error-messages="designationLevelErrors"
                                      @input="$v.formData.designationLevel.$touch()"
                                      @blur="$v.formData.designationLevel.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12">
                        <v-text-field label="Salary"
                                      v-model.number="formData.salary"
                                      type="number"
                                      :error-messages="salaryErrors"
                                      @input="$v.formData.salary.$touch()"
                                      @blur="$v.formData.salary.$touch()"
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
        name: "DesignationYearCreate",
        computed: {
            designationNameErrors() {
                const errors = []
                if (!this.$v.formData.designationName.$dirty) return errors
                !this.$v.formData.designationName.required && errors.push('Designation Name is required.')
                return errors
            },
            designationLevelErrors() {
                const errors = []
                if (!this.$v.formData.designationLevel.$dirty) return errors
                !this.$v.formData.designationLevel.required && errors.push('Designation Level is required.')
                return errors
            },
            salaryErrors() {
                const errors = []
                if (!this.$v.formData.salary.$dirty) return errors
                !this.$v.formData.salary.required && errors.push('Salary is required.')
                return errors
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
                formData: {},
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
            async saveData() {
                this.$v.$touch()
                if (this.$v.$invalid) {
                    this.submitStatus = 'ERROR'
                } else {
                    try {
                        const {data} = await axios.post('Designation/CreateDesignation', this.formData)
                        this.snackbar.message = data.success ? 'Designation Created Successfully' : data.errors.join(', ')
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

        },
        validations: {
            formData: {
                designationName: {required},
                designationLevel: {required},
                salary: {required},
            }
        },
    }
</script>

<style scoped>

</style>