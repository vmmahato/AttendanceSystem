<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Edit Section</span>
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
                        <v-text-field label="Section"
                                      v-model="formData.sectionName"
                                      :error-messages="sectionNameErrors"
                                      @input="$v.formData.sectionName.$touch()"
                                      @blur="$v.formData.sectionName.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12">
                        <v-text-field label="Section Code"
                                      v-model="formData.sectionCode"
                                      :error-messages="sectionCodeErrors"
                                      @input="$v.formData.sectionCode.$touch()"
                                      @blur="$v.formData.sectionCode.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12">
                        <v-autocomplete v-model="formData.departmentID"
                                        :items="departmentList"
                                        :error-messages="departmentIDErrors"
                                        @input="$v.formData.departmentID.$touch()"
                                        @blur="$v.formData.departmentID.$touch()"
                                        label="Department"
                                        clearable
                                        item-text="value"
                                        item-value="id"
                        ></v-autocomplete>
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
        name: "SectionEdit",
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
            sectionNameErrors() {
                const errors = []
                if (!this.$v.formData.sectionName.$dirty) return errors
                !this.$v.formData.sectionName.required && errors.push('Section Name is required.')
                return errors
            },
            sectionCodeErrors() {
                const errors = []
                if (!this.$v.formData.sectionCode.$dirty) return errors
                !this.$v.formData.sectionCode.required && errors.push('Section Code is required.')
                return errors
            },
            departmentIDErrors() {
                const errors = []
                if (!this.$v.formData.departmentID.$dirty) return errors
                !this.$v.formData.departmentID.required && errors.push('Department Name is required.')
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
                if(this.snackbar.color === 'error'){
                    this.snackbar.message = ''
                }
                this.$emit("closeDialog", this.snackbar)
                this.$v.$reset()
            },
            async saveData() {
                this.$v.$touch()
                if (this.$v.$invalid) {
                    this.submitStatus = 'ERROR'
                } else {
                    try {
                        const {data} = await axios.post('Section/UpdateSection', this.formData)
                        this.snackbar.message = data.success ? 'Section Updated Successfully' : data.errors.join(', ')
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
                    this.snackbar.message = e
                    this.snackbar.color = 'error'
                    this.snackbar.data = e
                    this.snackbar.snackbar = true
                }
            },
            async getFormData() {
                const {data} = await axios.get('Department/DDLDepartmentList')
                this.departmentList = data
            }
        },
        created() {
            this.getFormData()
            this.getEditData()
        },
        validations: {
            formData: {
                sectionName: {required},
                sectionCode: {required},
                departmentID: {required},
            }
        },
    }
</script>

<style scoped>

</style>