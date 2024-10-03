<template>
    <v-container>
        <v-card flat depressed class="customFontSize">
            <v-card-text>
                <v-row justify="center">
                    <v-col cols="12" md="6">
                        <v-autocomplete v-model="formData.reportType"
                                        :items="reportTypeList"
                                        item-value="value"
                                        item-text="id"
                                        :error-messages="reportTypeErrors"
                                        @input="$v.formData.reportType.$touch()"
                                        @blur="$v.formData.reportType.$touch()"
                                        label="Report Type"
                                        clearable
                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-menu
                                v-if="!checkDateLang"
                                ref="menu1"
                                v-model="snackbar.dateMenu1"
                                :close-on-content-click="false"
                                transition="scale-transition"
                                offset-y
                                min-width="290px"
                        >
                            <template v-slot:activator="{ on, attrs }">
                                <v-text-field
                                        v-model="formData.date"
                                        label="Date"
                                        readonly
                                        v-bind="attrs"
                                        v-on="on"
                                        clearable
                                        :error-messages="dateErrors"
                                        @input="$v.formData.date.$touch()"
                                        @blur="$v.formData.date.$touch()"
                                ></v-text-field>
                            </template>
                            <v-date-picker v-model="formData.date" no-title
                                           :max="maxDate"
                                           @input="snackbar.dateMenu1 = false"
                            ></v-date-picker>
                        </v-menu>
                        <nepaliDatePicker
                                v-else
                                label="Date"
                                refer="date"
                                v-model="formData.date"
                                :max="maxDate"
                                :error-messages="dateErrors"
                                @input="$v.formData.date.$touch()"
                                @blur="$v.formData.date.$touch()"
                        ></nepaliDatePicker>
                    </v-col>
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
                                        :error-messages="employeeIDErrors"
                                        @input="$v.formData.employeeID.$touch()"
                                        @blur="$v.formData.employeeID.$touch()"
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
                    <v-col md="3">
                        <v-btn color="teal" dark @click="generateReport">Generate Report</v-btn>
                    </v-col>
                </v-row>
            </v-card-text>
        </v-card>
        <template v-if="snackbar.reportComponent">
            <dailyAttendanceGrid v-if="isAttendanceFormat"
                                 :report="formData.reportType"
                                 :employeeID="formData.employeeID"
                                 :date="formData.date"
            />
            <dailyLeaveGrid v-if="isLeaveFormat"
                            :report="formData.reportType"
                            :employeeID="formData.employeeID"
                            :date="formData.date"
            />
            <dailyPunchGrid v-if="isPunchFormat"
                            :report="formData.reportType"
                            :employeeID="formData.employeeID"
                            :date="formData.date"
            />
            <dailyOfficeVisitGrid v-if="isOfficeVisitFormat"
                                  :report="formData.reportType"
                                  :employeeID="formData.employeeID"
                                  :date="formData.date"
            />
            <dailyMissingPunchGrid v-if="isMissingPunchFormat"
                                   :report="formData.reportType"
                                   :employeeID="formData.employeeID"
                                   :date="formData.date"
            />
        </template>
    </v-container>
</template>

<script>
    import {mapGetters} from "vuex"
    import dailyAttendanceGrid from './reportComponents/DailyAttendanceGrid'
    import dailyLeaveGrid from './reportComponents/DailyLeaveGrid'
    import dailyPunchGrid from './reportComponents/DailyPunchGrid'
    import dailyMissingPunchGrid from './reportComponents/DailyMissingPunchGrid'
    import dailyOfficeVisitGrid from './reportComponents/DailyOfficeVisitGrid'
    import nepaliDatePicker from "../../../../../components/nepaliDatePicker";
    import {required} from "vuelidate/lib/validators";
    import axios from "axios";

    export default {
        name: "DailyAttendanceReport",
        computed: {
            ...mapGetters(['getSystemUserData']),
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
            reportTypeErrors() {
                const errors = []
                if (!this.$v.formData.reportType.$dirty) return errors
                !this.$v.formData.reportType.required && errors.push('Report Type is required.')
                return errors
            },
            dateErrors() {
                const errors = []
                if (!this.$v.formData.date.$dirty) return errors
                !this.$v.formData.date.required && errors.push('Date is required.')
                return errors
            },
            employeeIDErrors() {
                const errors = []
                if (!this.$v.formData.employeeID.$dirty) return errors
                !this.$v.formData.employeeID.required && errors.push('Employee is required.')
                return errors
            },
            maxDate() {
                let currentDate = null;
                if (this.dateFormatNepali) {
                    let currentDateObject = window.NepaliFunctions.GetCurrentBsDate();
                    currentDate = window.NepaliFunctions.ConvertDateFormat(currentDateObject, "YYYY-MM-DD")
                } else {
                    currentDate = this.$moment().format('YYYY-MM-DD')
                }
                return currentDate
            },
            minDate() {
                let currentDate = '2077-06-01';
                return currentDate
            },
            isAttendanceFormat() {
                return this.formData.reportType === 'Attendance' ||
                    this.formData.reportType === 'Absent' ||
                    this.formData.reportType === 'EarlyIn' ||
                    this.formData.reportType === 'EarlyOut' ||
                    this.formData.reportType === 'LateIn' ||
                    this.formData.reportType === 'LateOut' ||
                    this.formData.reportType === 'OT';
            },
            isLeaveFormat() {
                return this.formData.reportType === 'Leave';
            },
            isPunchFormat() {
                return this.formData.reportType === 'ManualPunch';
            },
            isOfficeVisitFormat() {
                return this.formData.reportType === 'OfficeVisit';
            },
            isMissingPunchFormat() {
                return this.formData.reportType === 'MissingPunch';
            },
            ...mapGetters(['checkDateLang'])
        },
        data() {
            return {
                snackbar: {
                    snackbar: false,
                    timeOut: 5000,
                    dateMenu1: false,
                    loading: true,
                    dialog: false,
                    reportComponent: false,
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
                    reportType: null
                },
                dateFormatNepali: false,
                filterParams: {},
                reportTypeList: [
                    {
                        id: 'Attendance',
                        value: 'Attendance'
                    },
                    {
                        id: 'Early In',
                        value: 'EarlyIn'
                    },
                    {
                        id: 'Early Out',
                        value: 'EarlyOut'
                    },
                    {
                        id: 'Absent',
                        value: 'Absent'
                    },
                    {
                        id: 'Late In',
                        value: 'LateIn'
                    },
                    {
                        id: 'Late Out',
                        value: 'LateOut'
                    },
                    {
                        id: 'Missing Punch',
                        value: 'MissingPunch'
                    },
                    {
                        id: 'Leave',
                        value: 'Leave'
                    },
                    {
                        id: 'Manual Punch',
                        value: 'ManualPunch'
                    },
                    {
                        id: 'OT',
                        value: 'OT'
                    },
                    {
                        id: 'Office Visit',
                        value: 'OfficeVisit'
                    },
                ],
                tableHeader: [
                    {
                        text: 'Employee Detail',
                        value: '',
                        children: [
                            {
                                text: 'Code',
                                value: 'employee_code'
                            },
                            {
                                text: 'Name',
                                value: 'employee_name'
                            },
                        ],
                        rowspan: 1,
                        show: true
                    },
                    {
                        text: 'Estimated Schedule',
                        value: '',
                        children: [
                            {
                                text: 'In',
                                value: 'estimated_in'
                            },
                            {
                                text: 'Out',
                                value: 'estimated_out'
                            },
                            {
                                text: 'Work Hour',
                                value: 'estimated_work_hour'
                            },
                        ],
                        rowspan: 1,
                        show: true
                    },
                    {
                        text: 'Actual Schedule',
                        value: '',
                        children: [
                            {
                                text: 'In',
                                value: 'actual_in'
                            },
                            {
                                text: 'Out',
                                value: 'actual_out'
                            },
                            {
                                text: 'Break In',
                                value: 'actual_break_in'
                            },
                            {
                                text: 'Break Out',
                                value: 'actual_break_out'
                            },
                            {
                                text: 'Hours Worked',
                                value: 'actual_hours_worked'
                            },
                        ],
                        rowspan: 1,
                        show: true
                    },
                    {
                        text: 'ot',
                        value: 'ot',
                        children: [],
                        rowspan: 2,
                        show: true
                    },
                    {
                        text: 'Early',
                        value: '',
                        children: [
                            {
                                text: 'In',
                                value: 'early_in'
                            },
                            {
                                text: 'Out',
                                value: 'early_out'
                            },
                        ],
                        rowspan: 1,
                        show: true
                    },
                    {
                        text: 'Late',
                        value: '',
                        children: [
                            {
                                text: 'In',
                                value: 'late_in'
                            },
                            {
                                text: 'Out',
                                value: 'late_out'
                            },
                        ],
                        rowspan: 1,
                        show: true
                    },
                    {
                        text: 'Remarks',
                        value: 'remarks', children: [],
                        rowspan: 2,
                        show: true
                    },
                ],
                selectedHeaders: [
                    {
                        text: 'Employee Code',
                        value: 'employee_code'
                    },
                    {
                        text: 'Name',
                        value: 'employee_name'
                    },
                    {
                        text: 'Estimated In',
                        value: 'estimated_in'
                    },
                    {
                        text: 'Estimated Out',
                        value: 'estimated_out'
                    },
                    {
                        text: 'Estimated Work Hour',
                        value: 'estimated_work_hour'
                    },
                    {
                        text: 'Actual In',
                        value: 'actual_in'
                    },
                    {
                        text: 'Actual Out',
                        value: 'actual_out'
                    },
                    {
                        text: 'Actual Break In',
                        value: 'actual_break_in'
                    },
                    {
                        text: 'Actual Break Out',
                        value: 'actual_break_out'
                    },
                    {
                        text: 'Actual Hours Worked',
                        value: 'actual_hours_worked'
                    },
                    {
                        text: 'ot',
                        value: 'ot'
                    },
                    {
                        text: 'Late In',
                        value: 'late_in'
                    },
                    {
                        text: 'Late Out',
                        value: 'late_out'
                    },
                    {
                        text: 'Early In',
                        value: 'early_in'
                    },
                    {
                        text: 'Early Out',
                        value: 'early_out'
                    },
                    {
                        text: 'Remarks',
                        value: 'remarks'
                    },
                ],
                headersList: [
                    {
                        text: 'Employee Code',
                        value: 'employee_code'
                    },
                    {
                        text: 'Name',
                        value: 'employee_name'
                    },
                    {
                        text: 'Estimated In',
                        value: 'estimated_in'
                    },
                    {
                        text: 'Estimated Out',
                        value: 'estimated_out'
                    },
                    {
                        text: 'Estimated Work Hour',
                        value: 'estimated_work_hour'
                    },
                    {
                        text: 'Actual In',
                        value: 'actual_in'
                    },
                    {
                        text: 'Actual Out',
                        value: 'actual_out'
                    },
                    {
                        text: 'Actual Break In',
                        value: 'actual_break_in'
                    },
                    {
                        text: 'Actual Break Out',
                        value: 'actual_break_out'
                    },
                    {
                        text: 'Actual Hours Worked',
                        value: 'actual_hours_worked'
                    },
                    {
                        text: 'ot',
                        value: 'ot'
                    },
                    {
                        text: 'Late In',
                        value: 'late_in'
                    },
                    {
                        text: 'Late Out',
                        value: 'late_out'
                    },
                    {
                        text: 'Early In',
                        value: 'early_in'
                    },
                    {
                        text: 'Early Out',
                        value: 'early_out'
                    },
                    {
                        text: 'Remarks',
                        value: 'remarks'
                    },
                ],
                dataList: [],
                gate: null
            }
        },
        methods: {
            updateDate(value) {
                console.log('vale', value)
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
                })
            },
            async getEmployeeListByDepartmentSection() {
                const {data} = await axios.post('Employee/DepartmentAndSectionWiseEmployeeList', {
                    sectionID: this.formData.sectionID
                })
                this.employeeList = []
                this.employeeList = data
                this.formData.employeeID = this.formData.employeeID.filter(x => this.employeeList.filter(y => y.id === x).length)
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
            async generateReport() {
                this.$v.$touch()
                if (this.$v.$invalid) {
                    this.submitStatus = 'ERROR'
                    this.snackbar.reportComponent = false
                } else {
                    this.snackbar.reportComponent = true
                }
            },
        },
        created() {
            this.getDepartmentList()
        },
        watch: {
            'formData.departmentID': {
                handler() {
                    this.departmentWiseSectionAndEmployee()
                },
                deep: false,
            },
            'formData.sectionID': {
                handler() {
                    this.getEmployeeListByDepartmentSection()
                },
                deep: false
            },
        },
        components: {
            dailyAttendanceGrid,
            dailyMissingPunchGrid,
            nepaliDatePicker,
            dailyLeaveGrid,
            dailyPunchGrid,
            dailyOfficeVisitGrid
        },
        validations() {
            return {
                formData: {
                    reportType: {required},
                    date: {required},
                    employeeID: {required},
                }
            }
        }
    }
</script>

<style scoped lang="scss">
    .table-bordered tr th, td {
        border: 1px solid #ccc;
    }
</style>