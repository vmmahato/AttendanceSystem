<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Edit Manual Punch</span>
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
                    <v-col class="px-1" cols="12">
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
                                        <v-icon :color="formData.departmentID.length > 0 ? 'indigo darken-4' : ''">{{
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
                    <v-col class="px-1" cols="12">
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
                    <v-col class="px-1" cols="12">
                        <v-autocomplete v-model="formData.employeeId"
                                        :items="employeeList"
                                        label="Employee"
                                        chips
                                        small-chips
                                        deletable-chips
                                        item-value="id"
                                        item-text="value"
                                        clearable
                                        :error-messages="EmployeeIdErrors"
                                        @input="$v.formData.employeeId.$touch()"
                                        @blur="$v.formData.employeeId.$touch()"
                        >
                        </v-autocomplete>
                    </v-col>
                    <v-col cols="12">
                        <div class="d-flex flex-row">
                            <v-menu
                                    ref="dateMenu1"
                                    v-model="snackbar.dateMenu1"
                                    :close-on-content-click="false"
                                    transition="scale-transition"
                                    offset-y
                                    min-width="290px"
                                    v-if="!checkDateLang"
                            >
                                <template v-slot:activator="{ on, attrs }">
                                    <v-text-field
                                            v-model="formData.dateonly"
                                            label="Date/Time"
                                            readonly
                                            prepend-icon="event"
                                            v-bind="attrs"
                                            v-on="on"
                                            :error-messages="dateonlyErrors"
                                            @input="$v.formData.dateonly.$touch()"
                                            @blur="$v.formData.dateonly.$touch()"
                                    ></v-text-field>
                                </template>
                                <v-date-picker v-model="formData.dateonly" no-title
                                               @input="snackbar.dateMenu1 = false"></v-date-picker>
                            </v-menu>
                            <nepaliDatePicker
                                    v-else
                                    refer="date"
                                    v-model="formData.dateonly"
                                    :error-messages="dateonlyErrors"
                                    @input="$v.formData.dateonly.$touch()"
                                    @blur="$v.formData.dateonly.$touch()"
                            ></nepaliDatePicker>
                            <v-digital-time-picker
                                    v-model="formData.timeonly"
                                    prepend-icon="access_time"
                                    :error-messages="timeonlyErrors"
                                    @input="$v.formData.timeonly.$touch()"
                                    @blur="$v.formData.timeonly.$touch()"
                            />
                        </div>
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
    import nepaliDatePicker from "../../../../components/nepaliDatePicker";
    import { mapGetters } from 'vuex'

    export default {
        name: "ManualPunchEdit",
        components:{
            nepaliDatePicker
        },
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
            //EmployeeIdErrors() {
            //    const errors = []
            //    if (!this.$v.formData.EmployeeId.$dirty) return errors
            //    !this.$v.formData.EmployeeId.required && errors.push('Employee is required.')
            //    return errors
            //},
            dateonlyErrors() {
                const errors = []
                if (!this.$v.formData.dateonly.$dirty) return errors
                !this.$v.formData.dateonly.required && errors.push('Date is required.')
                return errors
            },
            timeonlyErrors() {
                const errors = []
                if (!this.$v.formData.timeonly.$dirty) return errors
                !this.$v.formData.timeonly.required && errors.push('Time is required.')
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
                    snackbar:false,
                    dateMenu1: false,
                    loaded:false
                },
                departmentList: [],
                sectionList: [],
                employeeList: [],
                formData: {
                    sectionID: [],
                    departmentID: [],
                    EmployeeId: null,
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
            async saveData() {
                this.$v.$touch()
                if (this.$v.$invalid) {
                    this.submitStatus = 'ERROR'
                } else {
                    try {
                        const {data} = await axios.post('ManualPuntch/UpdateManualPuntch', this.formData)
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
                    console.log('data',data)
                    this.formData = data
                    await this.getSectionList(this.formData.departmentID)
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
                    this.departmentWiseSectionAndEmployee()
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
            async getEmployeeListByDepartmentSection() {
                const {data} = await axios.post('Employee/DepartmentAndSectionWiseEmployeeList', {
                    sectionID: this.formData.sectionID
                })
                this.employeeList = []
                this.employeeList = data
            },
            async getSectionList() {
                const {data} = await axios.post('Section/DDLDepartmentWiseSectionListAsync', {
                    departmentID: this.formData.departmentID
                })
                this.sectionList = data
                this.formData.sectionID = this.formData.sectionID.filter(x => this.sectionList.filter(y => y.id === x).length)
            },
            async getDepartmentList() {
                const {data} = await axios.get('Department/DDLDepartmentList/')
                this.departmentList = data
            },
            async departmentWiseSectionAndEmployee() {
                await this.getSectionList()
            },
            getFormData() {
                this.getDepartmentList()
            },
        },
        created() {
            this.getFormData()
            this.getEditData()
        },
        validations: {
            formData: {
                dateonly: {required},
                timeonly: {required},
            }
        },
        watch: {
            'formData.departmentID': {
                handler() {
                    this.departmentWiseSectionAndEmployee()
                },
                deep: false,
                immediate:false
            },
            'formData.sectionID': {
                handler() {
                    this.getEmployeeListByDepartmentSection()
                },
                deep: false,
                immediate:false
            },
        },
    }
</script>

<style scoped>

</style>