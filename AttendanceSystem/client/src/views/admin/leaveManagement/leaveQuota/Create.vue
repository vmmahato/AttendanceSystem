<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Add Leave Quota</span>
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
                        <v-text-field label="Fiscal Year"
                                      v-model="formData.fiscalYear"
                                      :error-messages="fiscalYearErrors"
                                      @input="$v.formData.fiscalYear.$touch()"
                                      @blur="$v.formData.fiscalYear.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-menu
                                ref="menu1"
                                v-model="snackbar.dateMenu1"
                                :close-on-content-click="false"
                                transition="scale-transition"
                                offset-y
                                min-width="290px"
                        >
                            <template v-slot:activator="{ on, attrs }">
                                <v-text-field
                                        v-model="formData.startYear"
                                        label="Start Year"
                                        readonly
                                        v-bind="attrs"
                                        v-on="on"
                                        :error-messages="startYearErrors"
                                        @input="$v.formData.startYear.$touch()"
                                        @blur="$v.formData.startYear.$touch()"
                                ></v-text-field>
                            </template>
                            <v-date-picker v-model="formData.startYear" no-title
                                           @input="snackbar.dateMenu1 = false"></v-date-picker>
                        </v-menu>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-menu
                                ref="menu2"
                                v-model="snackbar.dateMenu2"
                                :close-on-content-click="false"
                                transition="scale-transition"
                                offset-y
                                min-width="290px"
                        >
                            <template v-slot:activator="{ on, attrs }">
                                <v-text-field
                                        v-model="formData.endYear"
                                        label="End Year"
                                        readonly
                                        v-bind="attrs"
                                        v-on="on"
                                        :error-messages="endYearErrors"
                                        @input="$v.formData.endYear.$touch()"
                                        @blur="$v.formData.endYear.$touch()"
                                ></v-text-field>
                            </template>
                            <v-date-picker v-model="formData.endYear" no-title
                                           @input="snackbar.dateMenu2 = false"></v-date-picker>
                        </v-menu>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-switch v-model="formData.isCurrentLeaveQuota"
                                  label="Is Fiscal Year?"
                                  clearable
                                  color="teal"
                        ></v-switch>
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
        name: "LeaveQuotaCreate",
        computed: {
            fiscalYearErrors() {
                const errors = []
                if (!this.$v.formData.fiscalYear.$dirty) return errors
                !this.$v.formData.fiscalYear.required && errors.push('Fiscal Year is required.')
                return errors
            },
            startYearErrors() {
                const errors = []
                if (!this.$v.formData.startYear.$dirty) return errors
                !this.$v.formData.startYear.required && errors.push('Start Year is required.')
                return errors
            },
            endYearErrors() {
                const errors = []
                if (!this.$v.formData.endYear.$dirty) return errors
                !this.$v.formData.endYear.required && errors.push('End Year is required.')
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
                        const {data} = await axios.post('LeaveQuota/CreateLeaveQuota', this.formData)
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

        },
        validations: {
            formData: {
                fiscalYear: {required},
                startYear: {required},
                endYear: {required},
            }
        },
    }
</script>

<style scoped>

</style>