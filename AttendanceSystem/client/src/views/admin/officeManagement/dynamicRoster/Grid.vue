<template>
    <v-container>
        <v-breadcrumbs :items="breadcrumb"></v-breadcrumbs>
        <v-row>
            <v-col cols="12" md="4">
                <v-autocomplete v-model="formData.fiscalYearID"
                                :items="fiscalYearList"
                                label="Fiscal Year"
                                chips
                                small-chips
                                item-value="id"
                                item-text="value"
                                clearable
                                @change="getFiscalYearData">
                </v-autocomplete>
            </v-col>

            <v-col cols="12" md="4" v-if="!checkDateLang">
                <v-autocomplete v-model="currentYear"
                                :items="yearList"
                                label="Year"
                                chips
                                small-chips
                                item-value="id"
                                item-text="value"
                                return-object
                                clearable
                                @change="getSelectedYear($event)">
                </v-autocomplete>
            </v-col>

            <v-col cols="12" md="4">
                <v-autocomplete v-model="formData.month" v-if="checkDateLang"
                                :items="monthList"
                                label="Month"
                                chips
                                small-chips
                                item-value="id"
                                item-text="value"
                                clearable>
                </v-autocomplete>
                <v-autocomplete v-model="formData.month" v-else
                                :items="englishMonthList"
                                label="Month"
                                chips
                                small-chips
                                item-value="id"
                                item-text="value"
                                clearable>
                </v-autocomplete>
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
                                clearable>
                    <template v-slot:prepend-item>
                        <v-list-item ripple
                                     @click="toggleDepartments">
                            <v-list-item-action>
                                <v-icon :color="formData.departmentID.length > 0 ? 'indigo darken-4' : ''">
                                    {{
                                    departmentIcon
                                    }}
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
                                clearable>
                    <template v-slot:prepend-item>
                        <v-list-item ripple
                                     @click="toggleSections">
                            <v-list-item-action>
                                <v-icon :color="formData.sectionID.length > 0 ? 'indigo darken-4' : ''">
                                    {{
                                    sectionIcon
                                    }}
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
                                @change="editItem(formData)">
                    <template v-slot:prepend-item>
                        <v-list-item ripple
                                     @click="toggleEmployees">
                            <v-list-item-action>
                                <v-icon :color="formData.employeeID.length > 0 ? 'indigo darken-4' : ''">
                                    {{
                                    employeeIcon
                                    }}
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
        <editForm :getItemUrl="itemDetailUrl" :employee-i-d="snackbar.employeeID"
                  :fiscal-year-i-d="snackbar.fiscalYearID"
                  :month="snackbar.month"
                  :year='snackbar.year'
                  :start-year="startYear"
                  :end-year="endYear"
                  :total-days="totalDays"
                  :start-date-bs="fullEnglishStartDate"
                  :end-date-bs="fullEnglishEndDate"
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
        name: "DynamicRosterGrid",
        computed: {
            ...mapGetters(['getSystemUserData', 'getTokenData']),
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
                        text: 'Dynamic Roster',
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
            },
            nepaliStartYearOnly(){
                return this.startDateBS.substring(0,4)
            },
            nepaliEndYearOnly(){
                return this.endDateBS.substring(0,4)
            },
            nepaliStartMonthOnly(){
                return this.startDateBS.substring(5,7)
            },
            YearSelected(){
                if(parseInt(this.snackbar.month)>=this.nepaliStartMonthOnly){
                    return this.nepaliStartYearOnly
                }
                return this.nepaliEndYearOnly
            },
            fullNepaliStartDate(){
                return this.YearSelected + '-'+this.formData.month +'-'+'01'
            },
            totalDays(){
                return window.NepaliFunctions.GetDaysInBsMonth(this.YearSelected, this.formData.month)
            },
            fullNepaliEndDate(){
                return this.YearSelected + '-'+this.formData.month +'-'+ this.totalDays
            },
            fullEnglishStartDate(){
                let nepaliDateObject = window.NepaliFunctions.ConvertToDateObject(this.fullNepaliStartDate, "YYYY-MM-DD")
                let englishDateObject = window.NepaliFunctions.BS2AD(nepaliDateObject, "YYYY-MM-DD")
                return window.NepaliFunctions.ConvertDateFormat(englishDateObject, "YYYY-MM-DD")
            },
            fullEnglishEndDate(){
                let nepaliDateObject = window.NepaliFunctions.ConvertToDateObject(this.fullNepaliEndDate, "YYYY-MM-DD")
                let englishDateObject = window.NepaliFunctions.BS2AD(nepaliDateObject, "YYYY-MM-DD")
                return window.NepaliFunctions.ConvertDateFormat(englishDateObject, "YYYY-MM-DD")
            },
            monthList(){
                if(this.checkDateLang){
                    return [
                        {id: '01', value: 'बैशाख'},
                        {id: '02', value: 'जेष्ठ'},
                        {id: '03', value: 'आशार'},
                        {id: '04', value: 'श्रावण'},
                        {id: '05', value: 'भाद्र'},
                        {id: '06', value: 'अश्विन'},
                        {id: '07', value: 'कार्तिक'},
                        {id: '08', value: 'मंग्सिर'},
                        {id: '09', value: 'पौष'},
                        {id: '10', value: 'माघ'},
                        {id: '11', value: 'फाल्गुन'},
                        {id: '12', value: 'चैत'},
                    ]
                }
                return  [
                    {id: '01', value: 'January'},
                    {id: '02', value: 'February'},
                    {id: '03', value: 'March'},
                    {id: '04', value: 'April'},
                    {id: '05', value: 'May'},
                    {id: '06', value: 'June'},
                    {id: '07', value: 'July'},
                    {id: '08', value: 'August'},
                    {id: '09', value: 'September'},
                    {id: '10', value: 'October'},
                    {id: '11', value: 'November'},
                    {id: '12', value: 'December'},
                ]
            },
            startDateAD(){
                return this.startYear.substring(0,4)
            },
            endDateAD(){
                return null
            },
            startYearAD(){
               return this.startYear.substring(0,4)
            },
            endYearAD(){
                 return this.endYear.substring(0,4)
            },
            startMonthAD(){
               return this.startYear.substring(5,7)
            },
            endMonthAD(){
                 return this.endYear.substring(5,7)
            },
            startDayAD(){
               return this.startYear.substring(8,10)
            },
            endDayAD(){
                 return this.endYear.substring(8,10)
            },
            ADYearList(){
                let returns = [ this.startYearAD, this.endYearAD ] 
                return returns
            },
            ...mapGetters(['checkDateLang'])
            //nepaliStartYearOnly(){}
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
                    fiscalYearID: null,
                    month: null,
                    adyear:null
                },
                yearList: [],
                currentYear: 0,
                selectedYear:0,
                departmentList: [],
                sectionList: [],
                employeeList: [],
                fiscalYearList: [],
                englishMonthList:[],
                formData: {
                    sectionID: [],
                    departmentID: [],
                    employeeID: [],
                    fiscalYear: null
                },
                filterParams: {},
                startYear: '',
                endYear: '',
                startDateBS: '',
                endDateBS: '',
            }
        },
        methods: {
            editItem({employeeID, month, fiscalYearID}) {
                this.snackbar.dialog = true
                this.snackbar.editForm = false
                this.snackbar.editForm = true
                this.snackbar.employeeID = employeeID
                this.snackbar.month = parseInt(month)
                this.snackbar.fiscalYearID = fiscalYearID
                this.snackbar.year = parseInt(this.selectedYear)
                //currentYear
                if(this.checkDateLang){
                    this.snackbar.getItemUrl = 'Roster/GetDynamicRosterListNPAsync'
                }
                else{
                    this.snackbar.getItemUrl = 'Roster/GetDynamicRosterListAsync'
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
                this.formData.fiscalYearID = parseInt(this.getTokenData.FiscalYearID)
                this.startYear =this.getTokenData.FromDateString
                this.endYear = this.getTokenData.ToDateString
                await this.getFiscalYearData()
            },
            async getFiscalYearList() {
                const {data} = await axios.get('FiscalYear/DDLFiscalYearList')
                this.fiscalYearList = data
            },
            async getFiscalYearData() {
                this.yearList = [];
                const {data} = await axios.get('FiscalYear/GetFiscalYearByID/' + this.formData.fiscalYearID)
                this.startYear = data.startYear
                this.endYear = data.endYear
                this.endDateBS = data.endDateBS
                this.startDateBS = data.startDateBS

                this.yearList.push({ 'id': 1, 'value': parseInt(data.startYear.slice(0, 4)) });
                 this.yearList.push({ 'id': 2, 'value': parseInt(data.endYear.slice(0, 4)) });
                this.yearList = [...new Set(this.yearList)];
            },
            async getSelectedYear(year) {
                 this.selectedYear = year.value;
                let currentMonth = 0;
                this.englishMonthList = [];
                if (year.id == 1) {
                  currentMonth = this.startYear.slice(5, 7);
                  this.englishMonthList = this.monthList.filter(function (data) {
                        return parseInt(data.id) >= parseInt(currentMonth);
                    })
                }
                else {
                      currentMonth = this.endYear.slice(5, 7);
                  this.englishMonthList = this.monthList.filter(function (data) {
                        return parseInt(data.id) <= parseInt(currentMonth);
                    })
                }
            },
            async departmentWiseSectionAndEmployee() {
                await this.getSectionList()
            },
        },
        created() {
            this.getDepartmentList()
            this.getFiscalYearList()
        },
        watch: {
            'formData.month': {
                handler() {
                    if(this.formData.employeeID.length){
                        this.editItem(this.formData)
                    }
                },
                deep: true,
                immediate:false,
            },
            'formData.fiscalYearID': {
                handler() {
                    if(this.formData.employeeID.length){
                        this.editItem(this.formData)
                    }
                },
                deep: true,
                immediate:false,
            },
            'formData.employeeID': {
                handler() {
                    this.editItem(this.formData)
                },
                deep: true,
                immediate:false,
            },
            checkDateLang: {
                handler() {
                    this.editItem(this.formData)
                },
                deep: true,
                immediate:true,
            },
            'formData.departmentID': {
                handler() {
                    this.departmentWiseSectionAndEmployee()
                },
                deep: true,
                immediate:false,
            },
            'formData.sectionID': {
                handler() {
                    this.getEmployeeListByDepartmentSection()
                },
                deep: true,
                immediate:false,
            },
        },
        components: {
            EditForm
        }
    }
</script>

<style scoped>

</style>