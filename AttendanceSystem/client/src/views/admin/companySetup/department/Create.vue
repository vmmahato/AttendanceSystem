<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Add Department</span>
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
                        <v-text-field label="Department"
                                      v-model="formData.departmentName"
                                      :error-messages="departmentNameErrors"
                                      @input="$v.formData.departmentName.$touch()"
                                      @blur="$v.formData.departmentName.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field label="Department Code"
                                      v-model="formData.departmentCode"
                                      :error-messages="departmentCodeErrors"
                                      @input="$v.formData.departmentCode.$touch()"
                                      @blur="$v.formData.departmentCode.$touch()"
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
        name: "FiscalYearCreate",
        computed: {
            departmentNameErrors() {
                const errors = []
                if (!this.$v.formData.departmentName.$dirty) return errors
                !this.$v.formData.departmentName.required && errors.push('Department Name is required.')
                return errors
            },
            departmentCodeErrors() {
                const errors = []
                if (!this.$v.formData.departmentCode.$dirty) return errors
                !this.$v.formData.departmentCode.required && errors.push('Department Code is required.')
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
                        const {data} = await axios.post('Department/CreateDepartment', this.formData)
                        this.snackbar.message = data.success ? 'Department Created Successfully' : data.errors.join(', ')
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
        },
        created() {

        },
        validations: {
            formData: {
                departmentName: {required},
                departmentCode: {required},
            }
        },
    }
</script>

<style scoped>

</style>