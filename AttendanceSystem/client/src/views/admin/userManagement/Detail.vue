<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">User Role Profile</span>
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
                    <v-col cols="12" md="8">
                        <v-row no-gutters>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="First Name"
                                              readonly
                                              v-model="formData.firstName"

                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="Last Name"
                                              readonly
                                              v-model="formData.lastName"

                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field
                                        v-model="formData.dateOfBirth"
                                        label="Date of Birth"
                                        readonly

                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field v-model="formData.gender"
                                              label="Gender"
                                              readonly

                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field v-model="formData.departmentName"
                                              label="Department"
                                              readonly

                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field v-model="formData.sectionName"
                                              label="Section"
                                              readonly

                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field v-model="formData.designationName"
                                              label="Designation"
                                              readonly

                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="Contact Number"
                                              readonly
                                              v-model="formData.phoneNumber"

                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="User Role Number"
                                              readonly
                                              v-model.number="formData.enrollID"
                                              type="number"

                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="Temporary Address"
                                              readonly
                                              v-model="formData.temporaryAddress"

                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="Permanent Address"
                                              readonly
                                              v-model="formData.permanentAddress"

                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field
                                        v-model="formData.joiningDate"
                                        label="Joining Date"
                                        readonly

                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="Status"
                                              readonly
                                              v-model.number="formData.status"
                                              type="number"

                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="6" class="px-1 ma-0">
                                <v-text-field label="Nationality"
                                              readonly
                                              v-model="formData.nationality"

                                ></v-text-field>
                            </v-col>
                        </v-row>
                    </v-col>
                    <v-col cols="12" md="4">
                        <v-row no-gutters>
                            <v-col cols="12">
                                <div id="preview" style="width: 200px">
                                    <v-img :src="formData.profileImageURL " alt="Upload Image"/>
                                </div>
                            </v-col>
                            <v-col cols="12">
                                <v-radio-group v-model="formData.checkClockIn"
                                               column
                                               label="Check Clock In"
                                               readonly
                                >
                                    <v-radio label="By Time Zone" value="true" class="pt-1"></v-radio>
                                    <v-radio label="Must Clock In" value="false"></v-radio>
                                    <v-radio label="Dont Check" value="true"></v-radio>
                                </v-radio-group>
                            </v-col>
                            <v-col cols="12">
                                <v-radio-group v-model="formData.checkClockOut"
                                               column
                                               label="Check Clock Out"
                                               readonly
                                >
                                    <v-radio label="By Time Zone" value="true" class="pt-1"></v-radio>
                                    <v-radio label="Must Clock In" value="false"></v-radio>
                                    <v-radio label="Dont Check" value="true"></v-radio>
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
                                        :error="!formData.countOT"
                                        label="Count OTs ?"
                                        readonly
                                        color="teal"
                                ></v-switch>
                            </v-col>
                            <v-col cols="12" md="4" class="ma-0 pa-0">
                                <v-switch
                                        class="ma-0 pa-0"
                                        v-model="formData.restOnHoliDays"
                                        label="Rest on Holidays?"
                                        readonly
                                        :error="!formData.restOnHoliDays"
                                        color="teal"
                                ></v-switch>
                            </v-col>
                            <v-col cols="12" md="4" class="ma-0 pa-0">
                                <v-switch
                                        class="ma-0 pa-0"
                                        v-model="formData.activeAccount"
                                        :error="!formData.activeAccount"
                                        label="Is Account Active?"
                                        readonly
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
        name: "RoleDetail",
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
                    snackbar: false,
                    data: {},
                    message: '',
                    color: '',
                    dateMenu1: null,
                    dateMenu2: null,
                },
                formData: {},
                genderList: [],
                departmentList: [],
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
        },
        created() {
            this.getEditData()
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