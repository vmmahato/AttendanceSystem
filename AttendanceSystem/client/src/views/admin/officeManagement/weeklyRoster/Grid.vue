<template>
    <v-container>
        <v-breadcrumbs :items="breadcrumb"></v-breadcrumbs>
        <v-row>
            <v-col cols="12" md="4">
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
            <v-col cols="12" md="4">
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
            <v-col cols="12" md="4">
                <v-autocomplete v-model="formData.employeeID"
                                :items="employeeList"
                                label="Employee"
                                multiple
                                chips
                                small-chips
                                deletable-chips
                                item-value="id"
                                item-text="value"
                                clearable
                                @change="editItem(formData)"
                >
                    <template v-slot:prepend-item>
                        <v-list-item
                                ripple
                                @click="toggleEmployees"
                        >
                            <v-list-item-action>
                                <v-icon :color="formData.employeeID.length > 0 ? 'indigo darken-4' : ''">{{
                                    employeeIcon }}
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
        </v-row>
        <editForm :getItemUrl="itemDetailUrl"
                  :employee-i-d="snackbar.employeeID"
                  v-if="formData.employeeID.length"
        />

        <v-snackbar :timeout="snackbar.timeOut"
                    :color="snackbar.color"
                    top
                    v-model="snackbar.snackbar">
            {{ snackbar.message }}
            <v-btn @click="snackbar.snackbar = false"
                   dark
                   text>
                Close
            </v-btn>
        </v-snackbar>
    </v-container>
</template>

<script>
    import {mapGetters} from "vuex"
    import axios from "axios";
    import EditForm from './Edit'

    export default {
        name: "WeeklyRosterGrid",
        computed: {
            ...mapGetters(['getSystemUserData']),
            breadcrumb() {
                return [
                    {
                        disabled: false,
                        exact: true,
                        text: 'Dashboard',
                        to: `${this.getSystemUserData.dashBoard}`
                    },
                    {
                        disabled: true,
                        exact: true,
                        text: 'Weekly Roster',
                        to: `${this.getSystemUserData.dashBoard}`
                    }
                ]
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
            selectAllEmployees() {
                return this.formData.employeeID.length === this.employeeList.length
            },
            selectSomeEmployees() {
                return this.formData.employeeID.length > 0 && !this.selectAllEmployees
            },
            employeeIcon() {
                if (this.selectAllEmployees) return 'mdi-close-box'
                if (this.selectSomeEmployees) return 'mdi-minus-box'
                return 'mdi-checkbox-blank-outline'
            },
            itemDetailUrl() {
                return this.snackbar.getItemUrl
            }
        },
        data() {
            return {
                snackbar: {
                    snackbar: false,
                    timeOut: 5000,
                    loading: true,
                    dialog: false,
                    createForm: false,
                    editForm: false,
                    deleteForm: false,
                    getItemUrl: '',
                    deleteUrl: '',
                    deleteItemTitle: '',
                    employeeID: [],
                },
                departmentList: [],
                sectionList: [],
                employeeList: [],
                formData: {
                    sectionID: [],
                    departmentID: [],
                    employeeID: [],
                },
                filterParams: {}
            }
        },
        methods: {
            editItem({employeeID}) {
                this.snackbar.dialog = true
                this.snackbar.editForm = true
                this.snackbar.employeeID = employeeID
                this.snackbar.getItemUrl = 'Roster/GetWeeklyRosterListAsync'
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
            toggleEmployees() {
                this.$nextTick(() => {
                    if (this.selectAllEmployees) {
                        this.formData.employeeID = []
                    } else {
                        this.formData.employeeID = this.employeeList.map(item => item.id)
                    }
                    this.editItem(this.formData)
                })
            },
            async getEmployeeListByDepartmentSection() {
                const {data} = await axios.post('Employee/DepartmentAndSectionWiseEmployeeList', {
                    sectionID: this.formData.sectionID
                })
                this.employeeList = []
                this.employeeList = data
                this.formData.employeeID = this.formData.employeeID.filter( x => this.employeeList.filter( y => y.id === x).length)
            },
            async getSectionList() {
                const {data} = await axios.post('Section/DDLDepartmentWiseSectionListAsync', {
                    departmentID: this.formData.departmentID
                })
                this.sectionList = data
                this.formData.sectionID = this.formData.sectionID.filter( x => this.sectionList.filter( y => y.id === x).length)
            },
            async getDepartmentList() {
                const {data} = await axios.get('Department/DDLDepartmentList/')
                this.departmentList = data
            },
            async departmentWiseSectionAndEmployee() {
                await this.getSectionList()
            },
            sectionChangeFunction() {
                //this.formData.employeeID = this.formData.employeeID.filter(x => this.employeeList.filter(y => y === x.id).length);
            }
        },
        created() {
            this.getDepartmentList()
        },
        watch: {
            'formData.employeeID': {
                handler() {
                    this.editItem(this.formData)
                },
                deep: true
            },
            'formData.departmentID': {
                handler() {
                    this.departmentWiseSectionAndEmployee()
                },
                deep: true
            },
            'formData.sectionID': {
                handler() {
                    this.getEmployeeListByDepartmentSection()
                },
                deep: true
            },
        },
        components: {
            EditForm
        }
    }
</script>

<style scoped>

</style>