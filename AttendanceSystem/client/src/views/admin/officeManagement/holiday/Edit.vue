<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Edit Holiday</span>
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
                <v-row v-if="snackbar.loaded">
                    <v-col cols="12" md="6">
                        <v-text-field label="Holiday Name"
                                      v-model="formData.holidayName"
                                      :error-messages="holidayNameErrors"
                                      @input="$v.formData.holidayName.$touch()"
                                      @blur="$v.formData.holidayName.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-switch v-model="formData.isWeekendLeave"
                                  label="Is Holiday Weekend?"
                                  clearable
                                  color="teal"
                        ></v-switch>
                    </v-col>
                    <v-col cols="12" md="6" v-if="isWeekendHoliday">
                        <v-autocomplete v-model="formData.weekendDay"
                                        :items="weekendDayList"
                                        :error-messages="weekendDayErrors"
                                        @input="$v.formData.weekendDay.$touch()"
                                        @blur="$v.formData.weekendDay.$touch()"
                                        label="Weekends"
                                        item-value="value"
                                        item-text="id"
                                        clearable
                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="6" v-if="!isWeekendHoliday">
                        <v-autocomplete v-model="formData.holidayType"
                                        :items="holidayTypeList"
                                        :error-messages="holidayTypeErrors"
                                        @input="$v.formData.holidayType.$touch()"
                                        @blur="$v.formData.holidayType.$touch()"
                                        label="Holiday Type"
                                        item-value="value"
                                        item-text="id"
                                        clearable
                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="6" v-if="!isWeekendHoliday">
                        <v-autocomplete v-model="formData.applicableReligion"
                                        :items="applicableReligionList"
                                        :error-messages="applicableReligionErrors"
                                        @input="$v.formData.applicableReligion.$touch()"
                                        @blur="$v.formData.applicableReligion.$touch()"
                                        label="Applicable Religion"
                                        item-value="value"
                                        item-text="id"
                                        clearable
                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="6" v-if="!isWeekendHoliday">
                        <v-autocomplete v-model="formData.applicableGender"
                                        :items="applicableGenderList"
                                        item-value="value"
                                        item-text="id"
                                        :error-messages="applicableGenderErrors"
                                        @input="$v.formData.applicableGender.$touch()"
                                        @blur="$v.formData.applicableGender.$touch()"
                                        label="Applicable Gender"
                                        clearable
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
                        <v-autocomplete v-model="formData.fiscalYear"
                                        :items="fiscalYearList"
                                        item-value="id"
                                        item-text="value"
                                        :error-messages="fiscalYearErrors"
                                        @input="$v.formData.fiscalYear.$touch()"
                                        @blur="$v.formData.fiscalYear.$touch()"
                                        label="Fiscal Year"
                                        clearable
                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-switch v-model="formData.isDepartmentWiseHoliday"
                                  label="Allow Department Wise Holiday?"
                                  clearable
                                  color="teal"
                        ></v-switch>
                    </v-col>
                    <template v-if="isAllowDepartmentWiseHoliday">
                        <v-col cols="12" md="6">
                            <v-autocomplete v-model="formData.departmentID"
                                            :items="departmentList"
                                            label="Departments"
                                            multiple
                                            chips
                                            small-chips
                                            deletable-chips
                                            item-value="id"
                                            item-text="value"
                                            clearable
                            >
                                <template v-slot:prepend-item>
                                    <v-list-item
                                            ripple
                                            @click="toggleDepartments"
                                    >
                                        <v-list-item-action>
                                            <v-icon :color="formData.departmentID.length > 0 ? 'indigo darken-4' : ''">
                                                {{
                                                departmentIcon }}
                                            </v-icon>
                                        </v-list-item-action>
                                        <v-list-item-content>
                                            <v-list-item-title>Select All</v-list-item-title>
                                        </v-list-item-content>
                                    </v-list-item>
                                    <v-divider class="mt-2"></v-divider>
                                </template>
                            </v-autocomplete>
                        </v-col>
                        <v-col cols="12" md="6">
                            <v-autocomplete v-model="formData.sectionID"
                                            :items="sectionList"
                                            label="Sections"
                                            multiple
                                            chips
                                            small-chips
                                            deletable-chips
                                            item-value="id"
                                            item-text="value"
                                            clearable
                            >
                                <template v-slot:prepend-item>
                                    <v-list-item
                                            ripple
                                            @click="toggleSections"
                                    >
                                        <v-list-item-action>
                                            <v-icon :color="formData.sectionID.length > 0 ? 'indigo darken-4' : ''">{{
                                                sectionIcon }}
                                            </v-icon>
                                        </v-list-item-action>
                                        <v-list-item-content>
                                            <v-list-item-title>Select All</v-list-item-title>
                                        </v-list-item-content>
                                    </v-list-item>
                                    <v-divider class="mt-2"></v-divider>
                                </template>
                            </v-autocomplete>
                        </v-col>
                    </template>
                    <v-col cols="12" md="6" v-if="!isWeekendHoliday">
                        <v-menu
                                ref="menu1"
                                v-model="snackbar.dateMenu3"
                                :close-on-content-click="false"
                                transition="scale-transition"
                                offset-y
                                min-width="290px"
                                v-if="!checkDateLang"
                        >
                            <template v-slot:activator="{ on, attrs }">
                                <v-text-field
                                        v-model="formData.fromDate"
                                        label="From"
                                        readonly
                                        v-bind="attrs"
                                        v-on="on"
                                        clearable
                                        :error-messages="fromDateErrors"
                                        @input="$v.formData.fromDate.$touch()"
                                        @blur="$v.formData.fromDate.$touch()"
                                ></v-text-field>
                            </template>
                            <v-date-picker v-model="formData.fromDate" no-title
                                           @input="snackbar.dateMenu3 = false"></v-date-picker>
                        </v-menu>
                        <nepaliDatePicker
                                v-else
                                refer="date"
                                label="From"
                                v-model="formData.fromDate"
                                :error-messages="fromDateErrors"
                                @input="$v.formData.fromDate.$touch()"
                                @blur="$v.formData.fromDate.$touch()"
                        ></nepaliDatePicker>
                    </v-col>
                    <v-col cols="12" md="6" v-if="!isWeekendHoliday">
                        <v-menu
                                ref="menu1"
                                v-model="snackbar.dateMenu4"
                                :close-on-content-click="false"
                                transition="scale-transition"
                                offset-y
                                min-width="290px"
                                v-if="!checkDateLang"
                        >
                            <template v-slot:activator="{ on, attrs }">
                                <v-text-field
                                        v-model="formData.toDate"
                                        label="To"
                                        readonly
                                        v-bind="attrs"
                                        v-on="on"
                                        clearable
                                        :error-messages="toDateErrors"
                                        @input="$v.formData.toDate.$touch()"
                                        @blur="$v.formData.toDate.$touch()"
                                ></v-text-field>
                            </template>
                            <v-date-picker v-model="formData.toDate" no-title
                                           @input="snackbar.dateMenu4 = false"></v-date-picker>
                        </v-menu>
                        <nepaliDatePicker
                                v-else
                                refer="date"
                                v-model="formData.toDate"
                                label="To"
                                :error-messages="toDateErrors"
                                @input="$v.formData.toDate.$touch()"
                                @blur="$v.formData.toDate.$touch()"
                        ></nepaliDatePicker>
                    </v-col>
                    <!-- <v-col cols="12">
                         <v-simple-table dense>
                             <template v-slot:default>
                                 <thead>
                                 <tr>
                                     <th class="text-left">Fiscal Year</th>
                                     <th class="text-left">From</th>
                                     <th class="text-left">To</th>
                                     <th class="text-left">
                                         <span>Actions</span>
                                         <v-spacer></v-spacer>
                                         <v-btn class="float-right" x-small right dark color="teal" @click="addSubmitYearList">
                                             <v-icon x-small dark>mdi-plus</v-icon>
                                         </v-btn>
                                     </th>
                                 </tr>
                                 </thead>
                                 <tbody>
                                 <template v-if="formData.submitYearList.length">
                                     <tr v-for="(item,index) in formData.submitYearList" :key="index" >
                                         <td>
                                             <v-select
                                                     :items="fiscalYearList"
                                                     label="Fiscal Year"
                                                     clearable
                                                     v-model="formData.submitYearList[index].fiscalYear"
                                             ></v-select>
                                         </td>
                                         <td>
                                             <v-menu
                                                     ref="menu1"
                                                     v-model="snackbar.dateMenu1[index]"
                                                     :close-on-content-click="false"
                                                     transition="scale-transition"
                                                     offset-y
                                                     min-width="290px"
                                             >
                                                 <template v-slot:activator="{ on, attrs }">
                                                     <v-text-field
                                                             v-model="formData.submitYearList[index].fromDate"
                                                             label="From"
                                                             readonly
                                                             v-bind="attrs"
                                                             v-on="on"
                                                             clearable
                                                     ></v-text-field>
                                                 </template>
                                                 <v-date-picker v-model="formData.submitYearList[index].fromDate" no-title
                                                                @input="snackbar.dateMenu1[index] = false"></v-date-picker>
                                             </v-menu>
                                         </td>
                                         <td>
                                             <v-menu
                                                     ref="menu1"
                                                     v-model="snackbar.dateMenu2[index]"
                                                     :close-on-content-click="false"
                                                     transition="scale-transition"
                                                     offset-y
                                                     min-width="290px"
                                             >
                                                 <template v-slot:activator="{ on, attrs }">
                                                     <v-text-field
                                                             v-model="formData.submitYearList[index].to"
                                                             label="To"
                                                             readonly
                                                             v-bind="attrs"
                                                             v-on="on"
                                                             clearable
                                                     ></v-text-field>
                                                 </template>
                                                 <v-date-picker v-model="formData.submitYearList[index].to" no-title
                                                                @input="snackbar.dateMenu2[index] = false"></v-date-picker>
                                             </v-menu>
                                         </td>
                                         <td>
                                             <v-btn class="float-right" x-small right dark color="red" @click="removeCurrentSubmitYear(index)">
                                                 <v-icon x-small dark>mdi-close</v-icon>
                                             </v-btn>
                                         </td>
                                     </tr>
                                 </template>
                                 </tbody>
                             </template>
                         </v-simple-table>
                     </v-col>-->
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
    import {required, requiredIf} from "vuelidate/lib/validators"
    import nepaliDatePicker from "../../../../components/nepaliDatePicker";
import { mapGetters } from 'vuex'

    export default {
        name: "HolidayEdit",
        props: {
            getItemUrl: {
                type: String,
                required: true
            }
        },
        components:{
            nepaliDatePicker
        },
        computed: {
            ItemDetailUrl() {
                return this.getItemUrl
            },
            isWeekendHoliday() {
                return this.formData.isWeekendLeave
            },
            isAllowDepartmentWiseHoliday() {
                return this.formData.isDepartmentWiseHoliday
            },
            selectAllDepartments() {
                return this.formData.departmentID.length === this.departmentList.length
            },
            selectSomeDepartments() {
                return this.formData.departmentID.length > 0 && !this.selectAllDepartments
            },
            departmentIcon() {
                if (this.selectAllDepartments) return 'mdi-close-box'
                if (this.selectSomeDepartments) return 'mdi-minus-box'
                return 'mdi-checkbox-blank-outline'
            },
            selectAllSections() {
                return this.formData.sectionID.length === this.sectionList.length
            },
            selectSomeSections() {
                return this.formData.sectionID.length > 0 && !this.selectAllSections
            },
            sectionIcon() {
                if (this.selectAllSections) return 'mdi-close-box'
                if (this.selectSomeSections) return 'mdi-minus-box'
                return 'mdi-checkbox-blank-outline'
            },
            holidayNameErrors() {
                const errors = []
                if (!this.$v.formData.holidayName.$dirty) return errors
                !this.$v.formData.holidayName.required && errors.push('Holiday Name is required.')
                return errors
            },
            holidayTypeErrors() {
                const errors = []
                if (!this.$v.formData.holidayType.$dirty) return errors
                !this.$v.formData.holidayType.required && errors.push('Holiday Type is required.')
                return errors
            },
            weekendDayErrors() {
                const errors = []
                if (!this.$v.formData.weekendDay.$dirty) return errors
                !this.$v.formData.weekendDay.required && errors.push('Field is required.')
                return errors
            },
            applicableReligionErrors() {
                const errors = []
                if (!this.$v.formData.applicableReligion.$dirty) return errors
                !this.$v.formData.applicableReligion.required && errors.push('Input is required.')
                return errors
            },
            applicableGenderErrors() {
                const errors = []
                if (!this.$v.formData.applicableGender.$dirty) return errors
                !this.$v.formData.applicableGender.required && errors.push('Gender Input is required.')
                return errors
            },
            fiscalYearErrors() {
                const errors = []
                if (!this.$v.formData.fiscalYear.$dirty) return errors
                !this.$v.formData.fiscalYear.required && errors.push('Fiscal Year is required.')
                return errors
            },
            fromDateErrors() {
                const errors = []
                if (!this.$v.formData.fromDate.$dirty) return errors
                !this.$v.formData.fromDate.required && errors.push('Date is required.')
                return errors
            },
            toDateErrors() {
                const errors = []
                if (!this.$v.formData.toDate.$dirty) return errors
                !this.$v.formData.toDate.required && errors.push('Date is required.')
                return errors
            },
            ...mapGetters(['checkDateLang'])
        },
        data() {
            return {
                snackbar: {
                    data: {},
                    message: '',
                    color: '',
                    dateMenu1:[],
                    dateMenu2:[],
                    dateMenu3:null,
                    dateMenu4:null,
                    loaded:false
                },
                holidayTypeList:['Religious','Cultural','National'],
                applicableReligionList:['Christianity','Islam','Hindu','Buddhism', 'All'],
                applicableGenderList:[],
                departmentList:[],
                sectionList:[],
                fiscalYearList:[],
                submitYearList: [],
                weekendDayList: [
                    {id: 'Sunday', value: 'SUNDAY'},
                    {id: 'Monday', value: 'MONDAY'},
                    {id: 'Tuesday', value: 'TUESDAY'},
                    {id: 'Wednesday', value: 'WEDNESDAY'},
                    {id: 'Thursday', value: 'THURSDAY'},
                    {id: 'Friday', value: 'FRIDAY'},
                    {id: 'Saturday', value: 'SATURDAY'},
                ],
                formData: {
                    sectionID:[],
                    departmentID:[],
                },
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
            async getDepartmentList() {
                let {data} = await axios.get('Department/DDLDepartmentList')
                this.departmentList = data
            },
            async getSectionList() {
                const {data} = await axios.post('Section/DDLDepartmentWiseSectionListAsync', {
                    departmentID: this.formData.departmentID
                })
                this.sectionList = data
                this.formData.sectionID = this.formData.sectionID.filter( x => this.sectionList.filter( y => y.id === x).length)
            },
            async getGenderList() {
                const {data} = await axios.get('Shared/GetGenderList')
                this.applicableGenderList = data
            },
            async getReligionList() {
                const {data} = await axios.get('Shared/GetApplicableReligionList')
                this.applicableReligionList = data
            },
            async getHolidayTypeList() {
                const {data} = await axios.get('Shared/GetHolidayTypeList')
                this.holidayTypeList = data
            },
            async getFiscalYearList() {
                const {data} = await axios.get('FiscalYear/DDLFiscalYearList')
                this.fiscalYearList = data
            },
            getFormData() {
                this.getDepartmentList()
                this.getGenderList()
                this.getReligionList()
                this.getHolidayTypeList()
                this.getFiscalYearList()
            },
            async saveData() {
                this.$v.$touch()
                if (this.$v.$invalid) {
                    this.submitStatus = 'ERROR'
                } else {
                    try {
                        const {data} = await axios.post('Holiday/UpdateHoliday', this.formData)
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
                this.snackbar.loaded = false
                try {
                    const {data} = await axios.get(this.ItemDetailUrl)
                    this.formData = data
                    this.snackbar.loaded = true
                } catch (e) {
                    console.log(e)
                }

            },
            toggleDepartments() {
                this.$nextTick(() => {
                    if (this.selectAllDepartments) {
                        this.formData.departmentID = []
                    } else {
                        this.formData.departmentID = this.departmentList.map(item => item.id)
                    }
                })
            },
            toggleSections() {
                this.$nextTick(() => {
                    if (this.selectAllSections) {
                        this.formData.sectionID = []
                    } else {
                        this.formData.sectionID = this.sectionList.map(item => item.id)
                    }
                })
            },
            addSubmitYearList() {
                this.formData.submitYearList.push(
                    {
                        fiscalYear: '',
                        fromDate: '',
                        toDate: '',
                    },
                )
            },
            removeCurrentSubmitYear(index) {
                if (index >= 0) {
                    this.formData.submitYearList.splice(index, 1)
                }
            }
        },
        created() {
            this.getFormData()
            this.getEditData()
        },
        validations() {
            return {
                formData: {
                    holidayName: {required},
                    holidayType: {
                        required: requiredIf(function () {
                            return !this.isWeekendHoliday
                        })
                    },
                    applicableReligion: {
                        required: requiredIf(function () {
                            return !this.isWeekendHoliday
                        })
                    },
                    applicableGender: {
                        required: requiredIf(function () {
                            return !this.isWeekendHoliday
                        })
                    },
                    fiscalYear: {required},
                    fromDate: {
                        required: requiredIf(function () {
                            return !this.isWeekendHoliday
                        })
                    },
                    toDate: {
                        required: requiredIf(function () {
                            return !this.isWeekendHoliday
                        })
                    },
                    weekendDay: {
                        required: requiredIf(function () {
                            return this.isWeekendHoliday
                        })
                    },
                }
            }
        },
        watch: {
            'formData.departmentID': {
                handler() {
                    this.getSectionList()
                },
                deep: true
            },
        },
    }
</script>

<style scoped>

</style>