<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Deductions</span>
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
                        <v-text-field label="Code"
                                      v-model="formData.deductionCode"
                                      :error-messages="deductionCodeErrors"
                                      @input="$v.formData.deductionCode.$touch()"
                                      @blur="$v.formData.deductionCode.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12">
                        <v-text-field label="Deduction Name"
                                      v-model="formData.deductionName"
                                      :error-messages="deductionNameErrors"
                                      @input="$v.formData.deductionName.$touch()"
                                      @blur="$v.formData.deductionName.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12">
                        <v-text-field label="Percentage"
                                      v-model.number="formData.deductionPercentage"
                                      type="number"
                                      :error-messages="deductionPercentageErrors"
                                      @input="$v.formData.deductionPercentage.$touch()"
                                      @blur="$v.formData.deductionPercentage.$touch()"
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
        name: "DeductionCreate",
        computed: {
            deductionPercentageErrors() {
                const errors = []
                if (!this.$v.formData.deductionPercentage.$dirty) return errors
                !this.$v.formData.deductionPercentage.required && errors.push('Percentage is required.')
                return errors
            },
            deductionCodeErrors() {
                const errors = []
                if (!this.$v.formData.deductionCode.$dirty) return errors
                !this.$v.formData.deductionCode.required && errors.push('Code is required.')
                return errors
            },
            deductionNameErrors() {
                const errors = []
                if (!this.$v.formData.deductionName.$dirty) return errors
                !this.$v.formData.deductionName.required && errors.push('Deduction is required.')
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
                formData: {},
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
            getFormData() {

            },
            async saveData() {
                this.$v.$touch()
                if (this.$v.$invalid) {
                    this.submitStatus = 'ERROR'
                } else {
                    try {
                        const {data} = await axios.post('Deduction/CreateDeduction', this.formData)
                        this.snackbar.message = data.success ? 'Deduction Created Successfully' : data.errors.join(', ')
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
                deductionPercentage: {required},
                deductionCode: {required},
                deductionName: {required},
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