<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Allowances</span>
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
                                      v-model="formData.allowanceCode"
                                      :error-messages="allowanceCodeErrors"
                                      @input="$v.formData.allowanceCode.$touch()"
                                      @blur="$v.formData.allowanceCode.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12">
                        <v-text-field label="Allowance Name"
                                      v-model="formData.allowanceName"
                                      :error-messages="allowanceNameErrors"
                                      @input="$v.formData.allowanceName.$touch()"
                                      @blur="$v.formData.allowanceName.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12">
                        <v-text-field label="Amount"
                                      v-model.number="formData.allowanceAmount"
                                      type="number"
                                      :error-messages="allowanceAmountErrors"
                                      @input="$v.formData.allowanceAmount.$touch()"
                                      @blur="$v.formData.allowanceAmount.$touch()"
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
        name: "AllowanceCreate",
        computed: {
            allowanceAmountErrors() {
                const errors = []
                if (!this.$v.formData.allowanceAmount.$dirty) return errors
                !this.$v.formData.allowanceAmount.required && errors.push('Amount is required.')
                return errors
            },
            allowanceCodeErrors() {
                const errors = []
                if (!this.$v.formData.allowanceCode.$dirty) return errors
                !this.$v.formData.allowanceCode.required && errors.push('Code is required.')
                return errors
            },
            allowanceNameErrors() {
                const errors = []
                if (!this.$v.formData.allowanceName.$dirty) return errors
                !this.$v.formData.allowanceName.required && errors.push('Allowance is required.')
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
                        const {data} = await axios.post('Allowance/CreateAllowance', this.formData)
                        this.snackbar.message = data.success ? 'Allowance Created Successfully' : data.errors.join(', ')
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
                allowanceAmount: {required},
                allowanceCode: {required},
                allowanceName: {required},
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