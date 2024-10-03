<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Edit Fiscal Year</span>
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
            <v-container v-if="snackbar.loaded">
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
                        <v-switch v-model="formData.isCurrentFiscalYear"
                                  label="Is Fiscal Year?"
                                  clearable
                                  color="teal"
                        ></v-switch>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-menu
                                ref="menu1"
                                v-model="snackbar.dateMenu1"
                                :close-on-content-click="false"
                                transition="scale-transition"
                                offset-y
                                min-width="290px"
                                v-if="!checkDateLang"
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
                                           :max="maxFromDate"
                                           @input="snackbar.dateMenu1 = false"></v-date-picker>
                        </v-menu>
                        <nepaliDatePicker
                                v-else
                                label="Start Year"
                                refer="date"
                                v-model="formData.startYear"
                                :max="maxFromDate"
                                :error-messages="startYearErrors"
                                @input="$v.formData.startYear.$touch()"
                                @blur="$v.formData.startYear.$touch()"
                        ></nepaliDatePicker>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-menu
                                v-if="!checkDateLang"
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
                                           :min="minToDate"
                                           @input="snackbar.dateMenu2 = false"></v-date-picker>
                        </v-menu>
                        <nepaliDatePicker
                                v-else
                                label="End Year"
                                refer="date"
                                v-model="formData.endYear"
                                :min="minToDate"
                                :error-messages="endYearErrors"
                                @input="$v.formData.endYear.$touch()"
                                @blur="$v.formData.endYear.$touch()"
                        ></nepaliDatePicker>
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
    import nepaliDatePicker from "../../../../components/nepaliDatePicker";
import { mapGetters } from 'vuex'
    import axios from 'axios'

    export default {
        name: "FiscalYearEdit",
        props: {
            getItemUrl: {
                type: String,
                required: true
            }
        },
        components: {
            nepaliDatePicker
        },
        computed: {
            ItemDetailUrl() {
                return this.getItemUrl
            },
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
            minToDate(){
                return this.formData.startYear
            },
            maxFromDate(){
                return this.formData.endYear
            },
            ...mapGetters(['checkDateLang'])
        },
        data() {
            return {
                snackbar: {
                    data: {},
                    message: '',
                    color: '',
                    snackbar: false,
                    loaded:false
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
                        let englishStartYearDateObject = window.NepaliFunctions.ConvertToDateObject(this.formData.startYear, "YYYY-MM-DD")
                        let englishEndYearDateObject = window.NepaliFunctions.ConvertToDateObject(this.formData.endYear, "YYYY-MM-DD")
                        let nepaliStartYearDateObject = window.NepaliFunctions.AD2BS(englishStartYearDateObject, "YYYY-MM-DD")
                        let nepaliEndYearDateObject = window.NepaliFunctions.AD2BS(englishEndYearDateObject, "YYYY-MM-DD")
                        this.formData.startDateBS = window.NepaliFunctions.ConvertDateFormat(nepaliStartYearDateObject, "YYYY-MM-DD")
                        this.formData.endDateBS = window.NepaliFunctions.ConvertDateFormat(nepaliEndYearDateObject, "YYYY-MM-DD")
                        const {data} = await axios.post('FiscalYear/UpdateFiscalYear', this.formData)
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
                }

            },
            async getEditData() {
                try {
                    this.snackbar.loaded = false
                    const {data} = await axios.get(this.ItemDetailUrl)
                    this.formData = data
                    this.snackbar.loaded = true
                } catch (e) {
                    console.log(e)
                }

            }
        },
        created() {
            this.getEditData()
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