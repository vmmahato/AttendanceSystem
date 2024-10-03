<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Edit Employee</span>
            <v-spacer></v-spacer>
            <!--<v-btn depressed text @click="closeForm">
                <v-icon>mdi-close</v-icon>
            </v-btn>-->
        </v-card-title>

        <v-card-text>
            <v-container>
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
                <v-row v-if="snackbar.loaded">
                    <v-col cols="12" md="8">
                        <v-row no-gutters>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="First Name"
                                              v-model="formData.firstName"
                                              :error-messages="firstNameErrors"
                                              @input="$v.formData.firstName.$touch()"
                                              @blur="$v.formData.firstName.$touch()"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="Last Name"
                                              v-model="formData.lastName"
                                              :error-messages="lastNameErrors"
                                              @input="$v.formData.lastName.$touch()"
                                              @blur="$v.formData.lastName.$touch()"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-autocomplete v-model="formData.deviceNumber"
                                                :items="deviceNumberList"
                                                item-value="value"
                                                item-text="value"
                                                :error-messages="deviceNumberErrors"
                                                @input="$v.formData.deviceNumber.$touch()"
                                                @blur="$v.formData.deviceNumber.$touch()"
                                                label="Device Number"
                                                clearable
                                ></v-autocomplete>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="Enroll ID"
                                              v-model.number="formData.enrollID"
                                              type="number"
                                              :error-messages="enrollIDErrors"
                                              @input="$v.formData.enrollID.$touch()"
                                              @blur="$v.formData.enrollID.$touch()"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
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
                                                v-model="formData.dateOfBirth"
                                                label="Date of Birth"
                                                readonly
                                                v-bind="attrs"
                                                v-on="on"
                                                :error-messages="dateOfBirthErrors"
                                                @input="$v.formData.dateOfBirth.$touch()"
                                                @blur="$v.formData.dateOfBirth.$touch()"
                                        ></v-text-field>
                                    </template>
                                    <v-date-picker v-model="formData.dateOfBirth" no-title
                                                   :max="maxDOB"
                                                   @input="snackbar.dateMenu1 = false"></v-date-picker>
                                </v-menu>
                                <nepaliDatePicker
                                        label="Date of Birth"
                                        v-else
                                        refer="date"
                                        v-model="formData.dateOfBirth"
                                        :error-messages="dateOfBirthErrors"
                                        @input="$v.formData.dateOfBirth.$touch()"
                                ></nepaliDatePicker>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-autocomplete v-model="formData.gender"
                                                :items="customGenderList"
                                                item-value="value"
                                                item-text="id"
                                                :error-messages="genderErrors"
                                                @input="$v.formData.gender.$touch()"
                                                @blur="$v.formData.gender.$touch()"
                                                label="Gender"
                                                clearable
                                ></v-autocomplete>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-autocomplete v-model="formData.departmentID"
                                                :items="departmentList"
                                                :error-messages="departmentIDErrors"
                                                @input="$v.formData.departmentID.$touch()"
                                                @blur="$v.formData.departmentID.$touch()"
                                                @change="getSectionList"
                                                label="Department"
                                                item-value="id"
                                                item-text="value"
                                                clearable
                                ></v-autocomplete>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-autocomplete v-model="formData.sectionID"
                                                :items="sectionList"
                                                :error-messages="sectionIDErrors"
                                                @input="$v.formData.sectionID.$touch()"
                                                @blur="$v.formData.sectionID.$touch()"
                                                label="Section"
                                                item-value="id"
                                                item-text="value"
                                                clearable
                                ></v-autocomplete>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-autocomplete v-model="formData.designationID"
                                                :items="designationList"
                                                :error-messages="designationIDErrors"
                                                @input="$v.formData.designationID.$touch()"
                                                @blur="$v.formData.designationID.$touch()"
                                                item-value="id"
                                                item-text="value"
                                                label="Designation"
                                                clearable
                                ></v-autocomplete>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="Contact Number"
                                              v-model="formData.phoneNumber"
                                              :error-messages="phoneNumberErrors"
                                              @input="$v.formData.phoneNumber.$touch()"
                                              @blur="$v.formData.phoneNumber.$touch()"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-autocomplete v-model="formData.religion"
                                                :items="religionList"
                                                item-value="value"
                                                item-text="id"
                                                :error-messages="religionErrors"
                                                @input="$v.formData.religion.$touch()"
                                                @blur="$v.formData.religion.$touch()"
                                                label="Religion"
                                                clearable
                                ></v-autocomplete>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="Temporary Address"
                                              v-model="formData.temporaryAddress"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="Permanent Address"
                                              v-model="formData.permanentAddress"
                                              :error-messages="permanentAddressErrors"
                                              @input="$v.formData.permanentAddress.$touch()"
                                              @blur="$v.formData.permanentAddress.$touch()"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-menu
                                        v-if="!checkDateLang"
                                        ref="menu1"
                                        v-model="snackbar.dateMenu2"
                                        :close-on-content-click="false"
                                        transition="scale-transition"
                                        offset-y
                                        min-width="290px"
                                >
                                    <template v-slot:activator="{ on, attrs }">
                                        <v-text-field
                                                v-model="formData.joiningDate"
                                                label="Joining Date"
                                                readonly
                                                v-bind="attrs"
                                                v-on="on"
                                                :error-messages="joiningDateErrors"
                                                @input="$v.formData.joiningDate.$touch()"
                                                @blur="$v.formData.joiningDate.$touch()"
                                        ></v-text-field>
                                    </template>
                                    <v-date-picker v-model="formData.joiningDate" no-title
                                                   :max="maxDOB"
                                                   @input="snackbar.dateMenu2 = false"></v-date-picker>
                                </v-menu>
                                <nepaliDatePicker
                                        label="Joining Date"
                                        v-else
                                        refer="date"
                                        v-model="formData.joiningDate"
                                        :error-messages="joiningDateErrors"
                                        @input="$v.formData.joiningDate.$touch()"
                                ></nepaliDatePicker>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ">
                                <v-switch
                                        v-model="formData.status"
                                        label="Status"
                                        clearable
                                        color="teal"
                                ></v-switch>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="Nationality"
                                              v-model="formData.nationality"
                                              :error-messages="nationalityErrors"
                                              @input="$v.formData.nationality.$touch()"
                                              @blur="$v.formData.nationality.$touch()"
                                ></v-text-field>
                            </v-col>
                        </v-row>
                    </v-col>
                    <v-col cols="12" md="4">
                        <v-row no-gutters>
                            <v-col cols="12">
                                <div id="preview" style="width: 200px">
                                    <v-img v-if="formData.employeeImage" :src="imageURL" alt="Upload Image"/>
                                </div>
                                <v-file-input @change="onFileChange"
                                              chips clearable
                                              label="Choose Image"
                                              v-model="formData.employeeImage"
                                ></v-file-input>
                            </v-col>
                            <v-col cols="12">
                                <v-radio-group v-model="formData.checkClockIn"
                                               column
                                               label="Check Clock In"
                                >
                                    <v-radio
                                            v-for="(item,value) in clockList"
                                            :key="'CheckClockIN'+value"
                                            :label="item.id" :value="item.value" class="pt-1"></v-radio>
                                </v-radio-group>
                            </v-col>
                            <v-col cols="12">
                                <v-radio-group v-model="formData.checkClockOut"
                                               column
                                               label="Check Clock Out"
                                >
                                    <v-radio
                                            v-for="(item,value) in clockList"
                                            :key="'CheckClockOUT'+value"
                                            :label="item.id" :value="item.value" class="pt-1"></v-radio>
                                </v-radio-group>
                            </v-col>
                        </v-row>
                    </v-col>
                    <v-col cols="12">
                        <v-row no-gutters>
                            <v-col cols="12" md="4" class="ma-0 pa-0">
                                <v-switch
                                        class="ma-0 pa-0"
                                        v-model="formData.countOT"
                                        label="Count OTs ?"
                                        clearable
                                        color="teal"
                                ></v-switch>
                            </v-col>
                            <v-col cols="12" md="4" class="ma-0 pa-0">
                                <v-switch
                                        class="ma-0 pa-0"
                                        v-model="formData.restOnHoliDays"
                                        label="Rest on Holidays?"
                                        clearable
                                        color="teal"
                                ></v-switch>
                            </v-col>
                            <v-col cols="12" md="4" class="ma-0 pa-0">
                                <v-switch
                                        class="ma-0 pa-0"
                                        v-model="formData.activeAccount"
                                        label="Is Account Active?"
                                        clearable
                                        color="teal"
                                ></v-switch>
                            </v-col>
                        </v-row>
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
    import {required, minValue, helpers, minLength, maxLength} from "vuelidate/lib/validators";
    import axios from 'axios'
    import nepaliDatePicker from "../../../../components/nepaliDatePicker";
import { mapGetters } from 'vuex'

    //const isPhone = helpers.regex('alpha', /^[9]\d{9}$/)
    const isPhone = helpers.regex('alpha',/^[0-9+\- ]*$/)
    const isNameValid = helpers.regex('alpha',/^[a-z_ ]*$/i)
    export default {
        name: "EmployeeEdit",
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
            firstNameErrors() {
                const errors = []
                if (!this.$v.formData.firstName.$dirty) return errors
                !this.$v.formData.firstName.required && errors.push('First Name is required.')
                !this.$v.formData.firstName.minLength && errors.push('minimum 2 character')
                !this.$v.formData.firstName.maxLength && errors.push('max 50 character')
                !this.$v.formData.firstName.isNameValid && errors.push('Must be Valid Name')
                return errors
            },
            lastNameErrors() {
                const errors = []
                if (!this.$v.formData.lastName.$dirty) return errors
                !this.$v.formData.lastName.required && errors.push('Last Name is required.')
                !this.$v.formData.lastName.minLength && errors.push('minimum 2 character')
                !this.$v.formData.lastName.maxLength && errors.push('maximum 50 character')
                !this.$v.formData.lastName.isNameValid && errors.push('Must be Valid Last Name')
                return errors
            },
            genderErrors() {
                const errors = []
                if (!this.$v.formData.gender.$dirty) return errors
                !this.$v.formData.gender.required && errors.push('Employee gender is required.')
                return errors
            },
            permanentAddressErrors() {
                const errors = []
                if (!this.$v.formData.permanentAddress.$dirty) return errors
                !this.$v.formData.permanentAddress.required && errors.push('Permanent Address is required.')
                return errors
            },
            designationIDErrors() {
                const errors = []
                if (!this.$v.formData.designationID.$dirty) return errors
                !this.$v.formData.designationID.required && errors.push('Designation is required.')
                return errors
            },
            sectionIDErrors() {
                const errors = []
                if (!this.$v.formData.sectionID.$dirty) return errors
                !this.$v.formData.sectionID.required && errors.push('Section is required.')
                return errors
            },
            departmentIDErrors() {
                const errors = []
                if (!this.$v.formData.departmentID.$dirty) return errors
                !this.$v.formData.departmentID.required && errors.push('Department is required.')
                return errors
            },
            phoneNumberErrors() {
                const errors = []
                if (!this.$v.formData.phoneNumber.$dirty) return errors
                !this.$v.formData.phoneNumber.required && errors.push('Contact Number is required.')
                !this.$v.formData.phoneNumber.isPhone && errors.push('Contact Number must be Valid.')
                !this.$v.formData.phoneNumber.minLength && errors.push('Minimum Length Requirement not met')
                !this.$v.formData.phoneNumber.maxLength && errors.push('Not a Valid Contact Number')
                return errors
            },
            enrollIDErrors() {
                const errors = []
                if (!this.$v.formData.enrollID.$dirty) return errors
                !this.$v.formData.enrollID.required && errors.push('Enroll ID is required.')
                !this.$v.formData.enrollID.minValue && errors.push('value must be positive non zero')
                return errors
            },
            dateOfBirthErrors() {
                const errors = []
                if (!this.$v.formData.dateOfBirth.$dirty) return errors
                !this.$v.formData.dateOfBirth.required && errors.push('Date of Birth is required.')
                return errors
            },
            joiningDateErrors() {
                const errors = []
                if (!this.$v.formData.joiningDate.$dirty) return errors
                !this.$v.formData.joiningDate.required && errors.push('Date of Join is required.')
                return errors
            },
            nationalityErrors() {
                const errors = []
                if (!this.$v.formData.nationality.$dirty) return errors
                !this.$v.formData.nationality.required && errors.push('Nationality is required.')
                return errors
            },
            deviceNumberErrors() {
                const errors = []
                if (!this.$v.formData.deviceNumber.$dirty) return errors
                !this.$v.formData.deviceNumber.required && errors.push('Device Number is required.')
                return errors
            },
            religionErrors() {
                const errors = []
                if (!this.$v.formData.religion.$dirty) return errors
                !this.$v.formData.religion.required && errors.push('Religion is required.')
                return errors
            },
            maxDOB(){
                return this.$moment().subtract(1,'days').format('YYYY-MM-DD')
            },
            customGenderList(){
                return this.genderList.filter(item=>item.value !== 'A')
            },
            ...mapGetters(['checkDateLang'])
        },
        data() {
            return {
                snackbar: {
                    data: {},
                    message: '',
                    color: '',
                    dateMenu1: null,
                    dateMenu2: null,
                    loaded:false
                },
                formData: {},
                genderList: [],
                departmentList: [],
                religionList: [],
                deviceNumberList: [],
                clockList: [],
                sectionList: [],
                designationList: [],
                imageURL: '',
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
            async getClockList() {
                let {data} = await axios.get('Employee/DDLCheckClockStatusList')
                this.clockList = data
            },
            async getDepartmentList() {
                let {data} = await axios.get('Department/DDLDepartmentList')
                this.departmentList = data
            },
            async getSectionList(id) {
                const {data} = await axios.get('Section/DDLSectionList/' + id)
                this.sectionList = data
            },
            async getDesignationList() {
                const {data} = await axios.get('Designation/DDLDesignationList')
                this.designationList = data
            },
            async getGenderList() {
                const {data} = await axios.get('Shared/GetGenderList')
                this.genderList = data
            },
            async getDeviceNumberList() {
                const {data} = await axios.get('Device/DDLDeviceList')
                this.deviceNumberList = data
            },
            async getReligionList() {
                const {data} = await axios.get('Shared/GetApplicableReligionList')
                this.religionList = data
            },
            getFormData() {
                this.getDepartmentList()
                this.getClockList()
                this.getDesignationList()
                this.getGenderList()
                this.getReligionList()
                this.getDeviceNumberList()
            },
            async saveData() {
                this.$v.$touch()
                if (this.$v.$invalid) {
                    this.submitStatus = 'ERROR'
                } else {
                    try {
                        let form_data = new FormData();
                        for (let key in this.formData) {
                            if(key !='modifiedTS' && key!='modifiedBy'){
                                form_data.append(key, this.formData[key]);
                            }
                        }
                        const {data} = await axios.post('Employee/UpdateEmployeeAsync', form_data)
                        this.snackbar.message = data.success ? 'Employee Updated Successfully' : data.errors.join(', ')
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
                    await this.getSectionList(data.departmentID)
                } catch (e) {
                    this.snackbar.message = e
                    this.snackbar.color = 'error'
                    this.snackbar.data = e
                    this.snackbar.snackbar = true
                }
            },
            onFileChange(e) {
                this.imageURL = URL.createObjectURL(e);
            }
        },
        created() {
            this.getFormData()
            this.getEditData()
        },
        validations: {
            formData: {
                firstName: {
                    required,
                    minLength: minLength(2),
                    maxLength: maxLength(50),
                    isNameValid
                },
                lastName: {
                    required,
                    minLength: minLength(2),
                    maxLength: maxLength(50),
                    isNameValid
                },
                gender: {required},
                permanentAddress: {required},
                departmentID: {required},
                phoneNumber: {
                    required,
                    isPhone,
                    maxLength: maxLength(15),
                    minLength: minLength(7)
                },
                enrollID: {
                    required,
                    minValue: minValue(1)
                },
                sectionID: {required},
                designationID: {required},
                dateOfBirth: {required},
                joiningDate: {required},
                nationality: {required},
                deviceNumber: {required},
                religion: {required},
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