<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Add Leave</span>
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
                        <v-text-field label="Leave"
                                      v-model="formData.leaveName"
                                      :error-messages="leaveNameErrors"
                                      @input="$v.formData.leaveName.$touch()"
                                      @blur="$v.formData.leaveName.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field label="Leave Code"
                                      v-model="formData.leaveCode"
                                      :error-messages="leaveCodeErrors"
                                      @input="$v.formData.leaveCode.$touch()"
                                      @blur="$v.formData.leaveCode.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field label="Number of Days"
                                      v-model.number="formData.leaveDays"
                                      type="number"
                                      :error-messages="leaveDaysErrors"
                                      @input="$v.formData.leaveDays.$touch()"
                                      @blur="$v.formData.leaveDays.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-autocomplete v-model="formData.leaveIncrement"
                                        :items="leaveIncrementList"
                                        label="Leave Increment"
                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-autocomplete v-model="formData.applicableGender"
                                        :items="applicableGenderList"
                                        item-value="value"
                                        item-text="id"
                                        label="Applicable Gender"
                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12">
                        <v-textarea
                                outlined
                                name="input-7-4"
                                label="Description"
                                v-model="formData.description"
                        ></v-textarea>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-switch v-model="formData.IsReplacementLeave"
                                  label="Is Replacement Leave?"
                                  clearable
                                  color="teal"
                        ></v-switch>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-switch v-model="formData.IsPaidLeave"
                                  label="Is Paid Leave?"
                                  clearable
                                  color="teal"
                        ></v-switch>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-switch v-model="formData.IsLeaveCarryable"
                                  label="Is Leave Carry able?"
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
    import {minValue, required} from "vuelidate/lib/validators";
    import axios from 'axios'

    export default {
        name: "LeaveSetupCreate",
        computed: {
            leaveNameErrors() {
                const errors = []
                if (!this.$v.formData.leaveName.$dirty) return errors
                !this.$v.formData.leaveName.required && errors.push('Leave Name is required.')
                return errors
            },
            leaveCodeErrors() {
                const errors = []
                if (!this.$v.formData.leaveCode.$dirty) return errors
                !this.$v.formData.leaveCode.required && errors.push('Leave Code is required.')
                return errors
            },
            leaveDaysErrors() {
                const errors = []
                if (!this.$v.formData.leaveDays.$dirty) return errors
                !this.$v.formData.leaveDays.minValue && errors.push('Leave Days is min 1')
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
                formData: {
                    applicableGender: "A",
                    leaveIncrement:"Yearly"
                },
                leaveIncrementList: ['Yearly','Monthly'],
                applicableGenderList: []
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
            async getGenderList() {
                const {data} = await axios.get('Shared/GetGenderList')
                this.applicableGenderList = data
            },
            async saveData() {
                this.$v.$touch()
                if (this.$v.$invalid) {
                    this.submitStatus = 'ERROR'
                } else {
                    try {
                        const {data} = await axios.post('LeaveSetup/CreateLeaveSetup', this.formData)
                        this.snackbar.message = data.success ? 'Leave Created Successfully' : data.errors.join(', ')
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
            getFormData(){
                this.getGenderList()
            }
        },
        created() {
            this.getFormData()
        },
        validations: {
            formData: {
                leaveName: {required},
                leaveCode: {required},
                leaveDays: {minValue: minValue(1)},
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