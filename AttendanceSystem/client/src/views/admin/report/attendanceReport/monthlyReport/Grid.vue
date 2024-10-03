<template>
    <v-container>
        <v-card flat depressed class="customFontSize">
            <v-card-text>
                <v-row justify="center" no-gutters>
                    <v-col class="px-1" cols="12" md="4">
                        <v-autocomplete v-model="formData.reportType"
                                        :items="reportTypeList"
                                        item-value="value"
                                        item-text="id"
                                        :error-messages="reportTypeErrors"
                                        @input="$v.formData.reportType.$touch()"
                                        @blur="$v.formData.reportType.$touch()"
                                        label="Report Type"
                                        clearable></v-autocomplete>
                    </v-col>
                    <v-col class="px-1" cols="12" md="4">
                        <v-autocomplete v-model="formData.fiscalYear"
                                        :items="fiscalYearList"
                                        item-value="id"
                                        item-text="value"
                                        :error-messages="fiscalYearErrors"
                                        @input="$v.formData.fiscalYear.$touch()"
                                        @blur="$v.formData.fiscalYear.$touch()"
                                        @change="getFiscalYearData"
                                        label="Fiscal Year"
                                        clearable></v-autocomplete>
                    </v-col>
                    <!-- <v-col class="px-1" cols="12" md="4">
                                  <v-autocomplete v-model="formData.month"
                                                  :items="monthList"
                                                  item-value="id"
                                                  item-text="value"
                                                  :error-messages="monthErrors"
                                                  @input="$v.formData.month.$touch()"
                                                  @blur="$v.formData.month.$touch()"
                                                  label="Month"
                                                  clearable
                                  ></v-autocomplete>
                              </v-col> -->
                    <v-col class="px-1" cols="12" md="4">
                        <v-menu v-if="!checkDateLang"
                                ref="menu1"
                                v-model="snackbar.dateMenu1"
                                :close-on-content-click="false"
                                transition="scale-transition"
                                offset-y
                                min-width="290px">
                            <template v-slot:activator="{ on, attrs }">
                                <v-text-field v-model="dateRange"
                                              label="Date Range"
                                              readonly
                                              v-bind="attrs"
                                              v-on="on"
                                              :error-messages="dateErrors"
                                              @input="$v.dateRange.$touch()"
                                              @blur="$v.dateRange.$touch()"></v-text-field>
                            </template>
                            <v-date-picker v-model="dateRange"
                                           no-title
                                           :max="maxDate"
                                           :min="minDate"
                                           scrollable
                                           reactive
                                           range
                                           @input="
                  dateRange.length === 2
                    ? (snackbar.dateMenu1 = false)
                    : (snackbar.dateMenu1 = true)
                "></v-date-picker>
                        </v-menu>
                        <v-layout v-else>
                            <nepaliDatePicker label="From"
                                              v-model="dateRange[0]"
                                              :min="minDate"
                                              :max="maxDate"
                                              :error-messages="dateErrors"
                                              @input="$v.dateRange.$touch()"
                                              @blur="$v.dateRange.$touch()"></nepaliDatePicker>
                            <nepaliDatePicker label="To"
                                              v-model="dateRange[1]"
                                              :error-messages="dateErrors"
                                              @input="$v.dateRange.$touch()"
                                              @blur="$v.dateRange.$touch()"
                                              :min="minDate"
                                              :max="maxDate"></nepaliDatePicker>
                        </v-layout>
                        <!--<nepaliRangePicker
                                            :error-messages="dateErrors"
                                            label-from="From Date"
                                            label-to="To Date"
                                            @input="$v.dateRange.$touch()"
                                            @blur="$v.dateRange.$touch()"
                                            v-model="dateRange"></nepaliRangePicker>-->
                    </v-col>
                    <v-col class="px-1" cols="12" md="4">
                        <v-autocomplete v-model="formData.departmentID"
                                        :items="departmentList"
                                        label="Departments"
                                        multiple
                                        chips
                                        small-chips
                                        deletable-chips
                                        item-value="id"
                                        item-text="value"
                                        clearable>
                            <template v-slot:prepend-item>
                                <v-list-item ripple @click="toggleDepartments">
                                    <v-list-item-action>
                                        <v-icon :color="
                                                formData.departmentID.length>
                                            0
                                            ? 'indigo darken-4'
                                            : ''
                                            "
                                            >{{ departmentIcon }}
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
                    <v-col class="px-1" cols="12" md="4">
                        <v-autocomplete v-model="formData.sectionID"
                                        :items="sectionList"
                                        label="Sections"
                                        multiple
                                        chips
                                        small-chips
                                        deletable-chips
                                        item-value="id"
                                        item-text="value"
                                        clearable>
                            <template v-slot:prepend-item>
                                <v-list-item ripple @click="toggleSections">
                                    <v-list-item-action>
                                        <v-icon :color="
                                                formData.sectionID.length>
                                            0 ? 'indigo darken-4' : ''
                                            "
                                            >{{ sectionIcon }}
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
                    <v-col class="px-1" cols="12" md="4">
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
                                        @blur="$v.formData.employeeID.$touch()">
                            <template v-slot:prepend-item>
                                <v-list-item ripple @click="toggleEmployees">
                                    <v-list-item-action>
                                        <v-icon :color="
                                                formData.employeeID.length>
                                            0 ? 'indigo darken-4' : ''
                                            "
                                            >{{ employeeIcon }}
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

                    <v-col class="px-1" md="3">
                        <v-btn color="teal" dark @click="generateReport">Generate Report</v-btn>
                    </v-col>
                </v-row>
            </v-card-text>
        </v-card>
        <template v-if="snackbar.reportComponent">
            <monthlyAttendanceGrid v-if="isAttendanceFormat"
                                   :report="formData.reportType"
                                   :employeeID="formData.employeeID"
                                   :fromDate="currentRange[0]"
                                   :toDate="currentRange[1]"
                                   :fiscalYear="formData.fiscalYear" />
            <monthlyLeaveGrid v-else-if="isLeaveFormat"
                              :report="formData.reportType"
                              :employeeID="formData.employeeID"
                              :fromDate="currentRange[0]"
                              :toDate="currentRange[1]"
                              :month="formData.month"
                              :fiscalYear="formData.fiscalYear" />
            <monthlyPunchGrid v-else-if="isPunchFormat"
                              :report="formData.reportType"
                              :employeeID="formData.employeeID"
                              :fromDate="currentRange[0]"
                              :toDate="currentRange[1]"
                              :month="formData.month"
                              :fiscalYear="formData.fiscalYear" />
            <monthlyOfficeVisitGrid v-else-if="isOfficeVisitFormat"
                                    :report="formData.reportType"
                                    :employeeID="formData.employeeID"
                                    :fromDate="currentRange[0]"
                                    :toDate="currentRange[1]"
                                    :month="formData.month"
                                    :fiscalYear="formData.fiscalYear" />
            <monthlyRosterGrid v-else-if="formData.reportType === 'Roster'"
                               :report="formData.reportType"
                               :employeeID="formData.employeeID"
                               :fromDate="currentRange[0]"
                               :toDate="currentRange[1]"
                               :month="formData.month"
                               :fiscalYear="formData.fiscalYear" />
            <monthlyMissingGrid v-else-if="formData.reportType === 'MissingPunch'"
                                :report="formData.reportType"
                                :employeeID="formData.employeeID"
                                :fromDate="currentRange[0]"
                                :toDate="currentRange[1]"
                                :month="formData.month"
                                :fiscalYear="formData.fiscalYear" />
            <span v-else></span>
        </template>
    </v-container>
</template>

<script>
    import { mapGetters } from "vuex";
    import monthlyOfficeVisitGrid from "./reportComponents/MonthlyOfficeVisitGrid";
    import monthlyAttendanceGrid from "./reportComponents/MonthlyAttendanceGrid";
    import monthlyLeaveGrid from "./reportComponents/MonthlyLeaveGrid";
    import monthlyRosterGrid from "./reportComponents/MonthlyRosterGrid";
    import monthlyPunchGrid from "./reportComponents/MonthlyPunchGrid";
    import monthlyMissingGrid from "./reportComponents/MonthlyMissingGrid";
    import nepaliDatePicker from "../../../../../components/nepaliDatePicker";
    import { required, minLength } from "vuelidate/lib/validators";
    import axios from "axios";

    const isProperRange = (flag, from, to) => () => (flag ? from <= to : true);

    export default {
        name: "ReportComponent",
        computed: {
            ...mapGetters(["getSystemUserData"]),
            selectAllDepartments() {
                return this.formData.departmentID.length === this.departmentList.length;
            },
            selectSomeDepartments() {
                return (
                    this.formData.departmentID.length > 0 && !this.selectAllDepartments
                );
            },
            departmentIcon() {
                if (this.selectAllDepartments) return "mdi-close-box";
                if (this.selectSomeDepartments) return "mdi-minus-box";
                return "mdi-checkbox-blank-outline";
            },
            selectAllSections() {
                return this.formData.sectionID.length === this.sectionList.length;
            },
            selectSomeSections() {
                return this.formData.sectionID.length > 0 && !this.selectAllSections;
            },
            sectionIcon() {
                if (this.selectAllSections) return "mdi-close-box";
                if (this.selectSomeSections) return "mdi-minus-box";
                return "mdi-checkbox-blank-outline";
            },
            selectAllEmployees() {
                return this.formData.employeeID.length === this.employeeList.length;
            },
            selectSomeEmployees() {
                return this.formData.employeeID.length > 0 && !this.selectAllEmployees;
            },
            employeeIcon() {
                if (this.selectAllEmployees) return "mdi-close-box";
                if (this.selectSomeEmployees) return "mdi-minus-box";
                return "mdi-checkbox-blank-outline";
            },
            reportTypeErrors() {
                const errors = [];
                if (!this.$v.formData.reportType.$dirty) return errors;
                !this.$v.formData.reportType.required &&
                    errors.push("Report Type is required.");
                return errors;
            },
            fiscalYearErrors() {
                const errors = [];
                if (!this.$v.formData.fiscalYear.$dirty) return errors;
                !this.$v.formData.fiscalYear.required &&
                    errors.push("Fiscal Year  is required.");
                return errors;
            },
            monthErrors() {
                const errors = [];
                if (!this.$v.formData.month.$dirty) return errors;
                !this.$v.formData.month.required && errors.push("Month is required.");
                return errors;
            },
            dateErrors() {
                const errors = [];
                if (!this.$v.dateRange.$dirty) return errors;
                !this.$v.dateRange.required &&
                    errors.push("Date (From - To) is required.");
                !this.$v.dateRange.minLength &&
                    errors.push("Both From and To Date Should be Selected");
                !this.$v.dateRange.isProperRange &&
                    errors.push("To Date Cant be earlier than From date");
                return errors;
            },
            employeeIDErrors() {
                const errors = [];
                if (!this.$v.formData.employeeID.$dirty) return errors;
                !this.$v.formData.employeeID.required &&
                    errors.push("Employee is required.");
                return errors;
            },
            maxDate() {
                // Clone the value before .endOf()
                // return this.$moment(this.dateInitial).endOf("month").format("YYYY-MM-DD");
                return this.endYear;
            },
            maxDateNepali() {
                // Clone the value before .endOf()
                // return this.$moment(this.dateInitial).endOf("month").format("YYYY-MM-DD");
                return this.endDateBS;
            },
            month() {
                return this.formData.month;
            },
            currentYear() {
                let index = this.formData.fiscalYear
                    ? this.fiscalYearList.findIndex(
                        (item) => item.id === this.formData.fiscalYear
                    )
                    : this.fiscalYearList.length - 1;
                return this.formData.fiscalYear
                    ? this.fiscalYearList[index].value.substr(0, 4)
                    : new Date().getFullYear();
            },
            currentYearNepali() {
                let index = this.formData.fiscalYear
                    ? this.fiscalYearList.findIndex(
                        (item) => item.id === this.formData.fiscalYear
                    )
                    : this.fiscalYearList.length - 1;
                return this.formData.fiscalYear
                    ? this.fiscalYearList[index].value.substr(0, 4)
                    : new Date().getFullYear();
            },
            dateInitial() {
                let month = this.month ? parseInt(this.month) - 1 : 0;
                return [this.currentYear, month];
            },
            minDate() {
                //   return this.$moment(this.dateInitial)
                //     .startOf("month")
                //     .format("YYYY-MM-DD");
                // Clone the value before .endOf()
                return this.startYear;
            },
            minDateNepali() {
                return this.$moment(this.dateInitial)
                    .startOf("month")
                    .format("YYYY-MM-DD");
                // Clone the value before .endOf()
            },
            currentRange() {
                return this.dateRange.length
                    ? this.dateRange
                    : [this.minDate, this.maxDate];
            },
            isAttendanceFormat() {
                return (
                    this.formData.reportType === "Attendance" ||
                    this.formData.reportType === "Absent" ||
                    this.formData.reportType === "EarlyIn" ||
                    this.formData.reportType === "EarlyOut" ||
                    this.formData.reportType === "LateIn" ||
                    this.formData.reportType === "LateOut" ||
                    this.formData.reportType === "OT"
                );
            },
            monthList() {
                if (!this.dateFormatNepali) {
                    return [
                        { id: 1, value: "January" },
                        { id: 2, value: "February" },
                        { id: 3, value: "March" },
                        { id: 4, value: "April" },
                        { id: 5, value: "May" },
                        { id: 6, value: "June" },
                        { id: 7, value: "July" },
                        { id: 8, value: "August" },
                        { id: 9, value: "September" },
                        { id: 10, value: "October" },
                        { id: 11, value: "November" },
                        { id: 12, value: "December" },
                    ];
                } else {
                    return [
                        { id: 1, value: "Baisakh" },
                        { id: 2, value: "Jestha" },
                        { id: 3, value: "Asar" },
                        { id: 4, value: "Shrawan" },
                        { id: 5, value: "Bhadra" },
                        { id: 6, value: "Asoj" },
                        { id: 7, value: "Kartik" },
                        { id: 8, value: "Mangsir" },
                        { id: 9, value: "Paush" },
                        { id: 10, value: "Magh" },
                        { id: 11, value: "Falgun" },
                        { id: 12, value: "Chaitra" },
                    ];
                }
            },
            isLeaveFormat() {
                return this.formData.reportType === "Leave";
            },
            isPunchFormat() {
                return this.formData.reportType === "ManualPunch";
            },
            isOfficeVisitFormat() {
                return this.formData.reportType === "OfficeVisit";
            },
            ...mapGetters(["checkDateLang"]),
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
                    getItemUrl: "",
                    deleteUrl: "",
                    deleteItemTitle: "",
                    employeeID: [],
                },
                year: null,
                departmentList: [],
                sectionList: [],
                employeeList: [],
                fiscalYearList: [],
                formData: {
                    sectionID: [],
                    departmentID: [],
                    employeeID: [],
                    reportType: null,
                    month: null,
                    fiscalYear: null,
                },
                startYear: null,
                endYear: null,
                endDateBS: null,
                startDateBS: null,
                dateRange: [],
                dateFormatNepali: false,
                reportTypeList: [
                    {
                        id: "Attendance",
                        value: "Attendance",
                    },
                    {
                        id: "Early In",
                        value: "EarlyIn",
                    },
                    {
                        id: "Early Out",
                        value: "EarlyOut",
                    },
                    {
                        id: "Absent",
                        value: "Absent",
                    },
                    {
                        id: "Late In",
                        value: "LateIn",
                    },
                    {
                        id: "Late Out",
                        value: "LateOut",
                    },
                    {
                        id: "Missing Punch",
                        value: "MissingPunch",
                    },
                    {
                        id: "Leave",
                        value: "Leave",
                    },
                    {
                        id: "Manual Punch",
                        value: "ManualPunch",
                    },
                    {
                        id: "OT",
                        value: "OT",
                    },
                    {
                        id: "Office Visit",
                        value: "OfficeVisit",
                    },
                    {
                        id: "Roster Report",
                        value: "Roster",
                    },
                ],
            };
        },
        methods: {
            setRangeDate() {
                /* this.dateRange = []
                           this.dateRange = [this.minDate, this.maxDate]
                           this.snackbar.dateMenu1 = true*/
            },
            toggleDepartments() {
                this.$nextTick(() => {
                    if (this.selectAllDepartments) {
                        this.formData.departmentID = [];
                    } else {
                        this.formData.departmentID = this.departmentList.map(
                            (item) => item.id
                        );
                    }
                    this.departmentWiseSectionAndEmployee();
                });
            },
            toggleSections() {
                this.$nextTick(() => {
                    if (this.selectAllSections) {
                        this.formData.sectionID = [];
                    } else {
                        this.formData.sectionID = this.sectionList.map((item) => item.id);
                    }
                });
            },
            toggleEmployees() {
                this.$nextTick(() => {
                    if (this.selectAllEmployees) {
                        this.formData.employeeID = [];
                    } else {
                        this.formData.employeeID = this.employeeList.map((item) => item.id);
                    }
                });
            },
            async getEmployeeListByDepartmentSection() {
                const { data } = await axios.post(
                    "Employee/DepartmentAndSectionWiseEmployeeList",
                    {
                        sectionID: this.formData.sectionID,
                    }
                );
                this.employeeList = [];
                this.employeeList = data;
                this.formData.employeeID = this.formData.employeeID.filter(
                    (x) => this.employeeList.filter((y) => y.id === x).length
                );
            },
            async getSectionList() {
                const { data } = await axios.post(
                    "Section/DDLDepartmentWiseSectionListAsync",
                    {
                        departmentID: this.formData.departmentID,
                    }
                );
                this.sectionList = data;
                this.formData.sectionID = this.formData.sectionID.filter(
                    (x) => this.sectionList.filter((y) => y.id === x).length
                );
            },
            async getDepartmentList() {
                const { data } = await axios.get("Department/DDLDepartmentList/");
                this.departmentList = data;
            },
            async departmentWiseSectionAndEmployee() {
                await this.getSectionList();
            },
            async getFiscalYearList() {
                const { data } = await axios.get("FiscalYear/DDLFiscalYearList");
                this.fiscalYearList = data;
            },
            async generateReport() {
                this.$v.$touch();
                if (this.$v.$invalid) {
                    this.submitStatus = "ERROR";
                    this.snackbar.reportComponent = false;
                } else {
                    this.snackbar.reportComponent = true;
                }
            },
            async getFiscalYearData() {
                const { data } = await axios.get(
                    "FiscalYear/GetFiscalYearByID/" + this.formData.fiscalYear
                );
                this.startYear = data.startYear;
                this.endYear = data.endYear;
                this.endDateBS = data.endDateBS;
                this.startDateBS = data.startDateBS;
            },
        },
        created() {
            this.getDepartmentList();
            this.getFiscalYearList();
        },
        watch: {
            "formData.departmentID": {
                handler() {
                    this.departmentWiseSectionAndEmployee();
                },
                deep: true,
            },
            "formData.sectionID": {
                handler() {
                    this.getEmployeeListByDepartmentSection();
                },
                deep: true,
            },
        },
        components: {
            monthlyAttendanceGrid,
            monthlyRosterGrid,
            monthlyLeaveGrid,
            monthlyPunchGrid,
            monthlyMissingGrid,
            monthlyOfficeVisitGrid,
            nepaliDatePicker,
        },
        validations() {
            return {
                formData: {
                    reportType: { required },
                    employeeID: { required },
                    fiscalYear: { required },
                },
                dateRange: {
                    required,
                    minLength: minLength(2),
                    isProperRange: isProperRange(
                        this.currentRange.length,
                        this.currentRange[0],
                        this.currentRange[1]
                    ),
                },
            };
        },
    };
</script>

<style scoped lang="scss">
    .table-bordered tr th,
    td {
        border: 1px solid #ccc;
    }
</style>
