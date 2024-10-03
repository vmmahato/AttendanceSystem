<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Employee Profile</span>
            <v-spacer></v-spacer>
            <v-btn depressed flat text @click="closeForm">
                <v-icon>mdi-close</v-icon>
            </v-btn>
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
                <v-row>
                    <v-col cols="12" md="8">
                        <v-row no-gutters>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="First Name"
                                              :value="formData.firstName"
                                              readonly
                                              filled
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="Last Name"
                                              :value="formData.lastName"
                                              readonly
                                              filled
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field
                                        :value="formData.dateOfBirth"
                                        readonly
                                        filled
                                        label="Date of Birth"
                                        v-bind="attrs"
                                        v-on="on"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-autocomplete v-model="formData.gender"
                                                :items="genderList"
                                                item-value="value"
                                                item-text="id"
                                                label="Gender"
                                                readonly
                                                filled
                                ></v-autocomplete>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-autocomplete :value="formData.departmentID"
                                                :items="departmentList"
                                                label="Department"
                                                item-value="id"
                                                item-text="value"
                                                readonly
                                                filled
                                ></v-autocomplete>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-autocomplete :value="formData.sectionID"
                                                :items="sectionList"
                                                label="Section"
                                                item-value="id"
                                                item-text="value"
                                                readonly
                                                filled
                                ></v-autocomplete>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-autocomplete :value="formData.designationID"
                                                :items="designationList"
                                                readonly
                                                filled
                                                item-value="id"
                                                item-text="value"
                                                label="Designation"
                                ></v-autocomplete>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="Contact Number"
                                              :value="formData.phoneNumber"
                                              readonly
                                              filled
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="Enroll ID"
                                              :value="formData.enrollID"
                                              readonly
                                              filled
                                              type="number"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="Temporary Address"
                                              :value="formData.temporaryAddress"
                                              readonly
                                              filled
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="Permanent Address"
                                              :value="formData.permanentAddress"
                                              readonly
                                              filled

                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                        <v-text-field
                                                :value="formData.joiningDate"
                                                readonly
                                                filled
                                                label="Joining Date"
                                                v-bind="attrs"
                                                v-on="on"

                                        ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ">
                                <v-switch
                                        :value="formData.status"
                                        readonly
                                        filled
                                        label="Status"

                                        color="teal"
                                ></v-switch>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="Nationality"
                                              :value="formData.nationality"
                                              readonly
                                              filled

                                ></v-text-field>
                            </v-col>
                        </v-row>
                    </v-col>
                    <v-col cols="12" md="4">
                        <v-row no-gutters>
                            <v-col cols="12">
                                <div id="preview" style="width: 200px">
                                    <v-img :src="imageURL || formData.profileImageURL" alt="Image"/>
                                </div>
                                <v-text-field chips
                                              label="Image"
                                              :value="formData.employeeImage"
                                              readonly
                                              filled
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12">
                                <v-radio-group :value="formData.checkClockIn"
                                               readonly
                                               filled
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
                                <v-radio-group :value="formData.checkClockOut"
                                               readonly
                                               filled
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
                                        :value="formData.countOT"
                                        readonly
                                        filled
                                        label="Count OTs ?"

                                        color="teal"
                                ></v-switch>
                            </v-col>
                            <v-col cols="12" md="4" class="ma-0 pa-0">
                                <v-switch
                                        class="ma-0 pa-0"
                                        :value="formData.restOnHolidays"
                                        readonly
                                        filled
                                        label="Rest on Holidays?"

                                        color="teal"
                                ></v-switch>
                            </v-col>
                            <v-col cols="12" md="4" class="ma-0 pa-0">
                                <v-switch
                                        class="ma-0 pa-0"
                                        :value="formData.activeAccount"
                                        readonly
                                        filled
                                        label="Is Account Active?"

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
            <v-btn color="red darken-1" text @click="closeForm">Close</v-btn>
        </v-card-actions>
    </v-card>
</template>

<script>
    import axios from 'axios'

    export default {
        name: "EmployeeDetail",
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
                genderList: [],
                departmentList: [],
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
            getFormData() {
                this.getDepartmentList()
                this.getClockList()
                this.getDesignationList()
                this.getGenderList()
            },
            async getEditData() {
                try {
                    const {data} = await axios.get(this.ItemDetailUrl)
                    this.formData = data
                    await this.getSectionList(data.departmentID)
                } catch (e) {
                    this.snackbar.message = e
                    this.snackbar.color = 'error'
                    this.snackbar.data = e
                    this.snackbar.snackbar = true
                }
            },
        },
        created() {
            this.getEditData()
            this.getFormData()
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