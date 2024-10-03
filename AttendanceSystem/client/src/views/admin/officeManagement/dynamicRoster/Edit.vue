<template>
    <div>
        <v-card  v-if="formData.length" class="customFontSize">
            <!-- <v-card-title>
                 <span class="headline">Dynamic Roster</span>
             </v-card-title>-->

            <v-card-text >
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
                    <v-row  justify="end" >
                        <v-col sm="auto">
                            <v-btn color="#5e61f7" dark @click="setApplicableDate"
                                   v-if="formData.length && snackbar.loaded"

                            >Set Applicable
                                date
                            </v-btn>
                        </v-col>
                        <v-col sm="auto">
                            <v-btn color="#009688" dark @click="setShifts"
                                   v-if="formData.length && snackbar.loaded"
                            >Set Shift
                            </v-btn>
                        </v-col>
                        <v-col sm="auto">
                            <v-btn color="yellow darken-4" dark @click="saveData" >Save</v-btn>
                        </v-col>
                        <v-col sm="auto">
                            <v-icon @click="getEditData"> mdi-reload </v-icon>
                        </v-col>
                    </v-row>
                    <v-simple-table fixed-header height="500px">
                        <template v-slot:default>
                            <thead>
                            <tr>
                                <th class="text-left">Employee</th>
                                <th class="text-left">Applicable Date</th>
                                <template  v-if="!checkDateLang">
                                    <th v-for="(it,ct) in headerData" :key="'header'+ct"
                                        class="text-left"
                                    >
                                        {{it}}
                                    </th>
                                </template>
                                <template v-else>
                                    <th v-for="it in totalDays" :key="'header'+it"
                                        class="text-left"
                                    >
                                        {{it}}
                                    </th>
                                </template>
                            </tr>
                            </thead>
                            <tbody>
                            <tr v-for="(item,index) in formData" :key="index">
                                <td class="text-left">{{item.name}}</td>
                                <td>
                                    <v-menu
                                            v-model="snackbar.dateMenu[index]"
                                            class="mt-3"
                                            outlined
                                            :close-on-content-click="false"
                                            transition="scale-transition"
                                            offset-y
                                            min-width="290px"
                                            v-if="!checkDateLang"
                                    >
                                        <template v-slot:activator="{ on, attrs }">
                                            <v-text-field
                                                    v-model="item.applicableDateString"
                                                    class="mt-3"
                                                    outlined
                                                    label="Applicable Date"
                                                    readonly
                                                    v-bind="attrs"
                                                    v-on="on"
                                            ></v-text-field>
                                        </template>
                                        <v-date-picker v-model="item.applicableDateString" no-title
                                                       class="mt-3"
                                                       outlined
                                                       :min="minDate"
                                                       :max="maxDate"
                                                       @input="snackbar.dateMenu[index] = false"></v-date-picker>
                                    </v-menu>
                                    <nepaliDatePicker
                                            v-if="snackbar.dateLoader && checkDateLang"
                                            v-model="item.applicableDateString"
                                            :min="minDate"
                                            :max="maxDate"
                                            :outlined="true"
                                    ></nepaliDatePicker>
                                </td>
                                <td v-for="(listCode,count) in item.employeeShiftList" :key="count+'listCode'+index">
                                    <v-autocomplete v-model="listCode.shiftID"
                                                    class="mt-3"
                                                    outlined
                                                    :items="listCode.shiftList"
                                                    item-value="id"
                                                    item-text="value"
                                                    label="Shift"
                                                    clearable
                                                    :disabled="listCode.isOldDate"
                                    ></v-autocomplete>
                                </td>
                            </tr>
                            </tbody>
                        </template>
                    </v-simple-table>
                </v-container>
            </v-card-text>

           <!-- <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="teal darken-1" dark @click="saveData" v-if="!isOldMonth">Save</v-btn>
            </v-card-actions>-->
            <v-dialog
                    persistent
                    transition="dialog-bottom-transition"
                    max-width="400"
                    v-model="snackbar.dialog"
                    v-if="snackbar.dialog">
                <v-card class="customFontSize">
                    <v-card-title>
                        <span class="subtitle-1 text-center">Set applicable Date For All Employees</span>
                    </v-card-title>
                    <v-card-text >
                        <v-date-picker v-model="snackbar.picker"
                                       :min="minDate"
                                       :max="maxDate"
                                       color="teal"
                                       v-if="!checkDateLang"
                        ></v-date-picker>
                        <nepaliDatePicker
                                v-else
                                v-model="snackbar.picker"
                                :min="minDate"
                                :max="maxDate"
                                :outlined="true"
                        ></nepaliDatePicker>
                    </v-card-text>
                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn color="red darken-1" text @click="closeDateSetter">Cancel</v-btn>
                        <v-btn color="teal darken-1" text @click="setDates" :disabled="!snackbar.picker">Save</v-btn>
                    </v-card-actions>
                </v-card>
            </v-dialog>
            <v-dialog
                    persistent
                    transition="dialog-bottom-transition"
                    max-width="400"
                    v-model="snackbar.dialogShift"
                    v-if="snackbar.dialogShift">
                <v-card class="customFontSize">
                    <v-card-title>
                        <span class="subtitle-1 text-center">Set Shifts For All Employees</span>
                    </v-card-title>
                    <v-card-text>
                        <v-autocomplete v-model="snackbar.shift"
                                        :items="shiftList"
                                        item-value="id"
                                        item-text="value"
                                        label="Shift"
                                        clearable
                        ></v-autocomplete>
                    </v-card-text>
                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn color="red darken-1" text @click="closeDateSetter">Cancel</v-btn>
                        <v-btn color="teal darken-1" text @click="setShift" :disabled="!snackbar.shift">Save</v-btn>
                    </v-card-actions>
                </v-card>
            </v-dialog>
        </v-card>
        <v-progress-linear
                color="red lighten-2"
                buffer-value="0"
                stream
                v-if="loadingData && !formData.length"
        ></v-progress-linear>
    </div>
</template>

<script>
    import axios from 'axios'
    import { mapGetters } from 'vuex'
    import nepaliDatePicker from "../../../../components/nepaliDatePicker";
    export default {
        name: "DynamicRosterEdit",
        props: {
            getItemUrl: {
                type: String,
                required: true
            },
            employeeID: {
                type: Array,
                nullable: true
            },
            fiscalYearID: {
                type: Number,
                nullable: true
            },
              year: {
                type: Number,
                nullable: true
            },
            month: {
                nullable: true
            },
            totalDays: {
                nullable: true
            },
            startYear: {
                type: String,
                nullable: true
            },
            endYear: {
                type: String,
                nullable: true
            },
            startDateBs: {
                type: String,
                nullable: true
            },
            endDateBs: {
                type: String,
                nullable: true
            }
        },
        watch: {
            employeeID: {
                handler() {
                    this.getEditData()
                },
                deep: true
            },
            month: {
                handler() {
                    this.getEditData()
                },
                deep: true
            },
              year: {
                handler() {
                    this.getEditData()
                },
                deep: true
            },
            fiscalYearID: {
                handler() {
                    this.getEditData()
                },
                deep: true
            },
            'snackbar.dialogShift': {
                handler() {
                    this.getShiftData()
                },
            },
            getItemUrl:{
                handler() {
                    this.getEditData()
                },
                deep: true
            }
        },
        computed: {
            ItemDetailUrl() {         
                 if(this.checkDateLang){
                   return 'Roster/GetDynamicRosterListNPAsync'
                }
                else{
                   return 'Roster/GetDynamicRosterListAsync'
                }
               
            },
            dateToday() {
                return this.$moment().add(1, 'days').format('YYYY-MM-DD')
            },
            startYearDate(){
                return this.startYear
            },
            endYearDate(){
                return this.endYear
            },
            minDate(){
                if (this.dateToday > this.startYearDate && this.dateToday < this.endYearDate)
                {
                    
                    return this.dateToday
                }else{
                    if(this.checkDateLang){
                        return this.startDateBs
                    }
                    return this.startYearDate
                }
            },
            maxDate(){
                if(this.checkDateLang){
                    return this.endYear
                }
                return this.endYearDate
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
                    dateMenu: [false],
                    picker: null,
                    shift: null,
                    dialog: false,
                    dialogShift: false,
                    loaded:false,
                    dateLoader:true
                },
                loadingData:false,
                formData: [],
                headerData: [],
                nepaliFromDate: null,
                nepaliToDate: null,
                englishFromDate: null,
                englishToDate: null,
                shiftList: [],
                submitStatus: '',
              //  isOldMonth:false
            }
        },
        methods: {
            closeForm() {
                if(this.snackbar.color === 'error'){
                    this.snackbar.message = ''
                }
                this.$emit("closeDialog", this.snackbar)
            },
            async saveData() {

                try {
                    let url = ''
                    if(this.checkDateLang){
                        url = 'Roster/UpdateDynamicRosterNPAsync'
                    }
                    else{
                        url = 'Roster/UpdateDynamicRosterAsync'
                    }
                    const {data} = await axios.post(url, {
                        employeeShiftList:this.formData,
                        fiscalYear: this.fiscalYearID,
                        nepaliFromDate:this.nepaliFromDate,
                        nepaliToDate:this.nepaliToDate,
                        englishFromDate:this.englishFromDate,
                        englishToDate:this.englishToDate,
                        month:this.month,
                    })
                    this.snackbar.message = data.success ? 'Data Updated Successfully' : data.errors.join(', ')
                    this.snackbar.color = data.success ? 'success' : 'error'
                    this.snackbar.data = data
                    console.log(data)
                    if (data.success) {
                        this.snackbar.snackbar = true
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
            },
            async getEditData() {
                this.loadingData =true
                this.snackbar.loaded =false
                 this.snackbar.dateLoader =false
                try {
                    let params = {}
                    if(this.checkDateLang){
                        params = {
                            employeeList: this.employeeID,
                            fiscalYearID: this.fiscalYearID,
                            NepaliFromDate:this.startDateBs,
                            NepaliToDate:this.endDateBs
                        }
                    }
                    else{
                        params = {
                            employeeList: this.employeeID,
                            fiscalYearID: this.fiscalYearID,
                            year:this.year,
                            month:this.month,
                        }
                    }
                    const {data} = await axios.post(this.ItemDetailUrl, params)
                    this.formData = data.employeeShiftList
                    this.headerData = data.days
                    this.nepaliFromDate = data.nepaliFromDate
                    this.nepaliToDate = data.nepaliToDate
                    if (!this.checkDateLang) {
                        this.englishFromDate = this.$moment(data.englishFromDate).format('YYYY-MM-DD')
                        this.englishToDate = this.$moment(data.englishToDate).format('YYYY-MM-DD')
                    }
                  //  this.isOldMonth = data.isOldMonth
                    this.loadingData = false
                    this.snackbar.loaded =true
                    this.snackbar.dateLoader =true
                } catch (e) {
                    console.log(e)
                    this.loadingData = false
                }
            },
            async getShiftData() {
                try {
                    const {data} = await axios.get('Roster/GetShiftListAsync')
                    this.shiftList = data
                } catch (e) {
                    console.log(e)
                }

            },
            setApplicableDate() {
                this.snackbar.dialog = true
            },
            setShifts() {
                this.snackbar.dialogShift = true
            },
            closeDateSetter() {
                this.snackbar.dialog = false
                this.snackbar.dialogShift = false
            },
            async setDates() {
                this.snackbar.dateLoader = false
                await this.formData.forEach(item => {
                    item.applicableDateString = this.snackbar.picker
                })
                this.snackbar.dateLoader = true
                this.closeDateSetter()
            },
            setShift() {
                this.formData.forEach(item => {
                    item.employeeShiftList.forEach(it=>{
                       if(!it.isOldDate){
                           it.shiftID = this.snackbar.shift
                       }
                    })
                })
                this.closeDateSetter()
            },
        },
        created() {
            this.getEditData()
        },
        components:{
            nepaliDatePicker
        },
    }
</script>

<style scoped lang="scss">
    ::v-deep.theme--light.v-data-table>.v-data-table__wrapper>table>thead>tr>th {
        color: #f5f5f5;
    }
    ::v-deep.theme--light.v-data-table.v-data-table--fixed-header thead th {
        box-shadow: inset 0 -1px 0 rgba(0,0,0,.12);
        background: #009688!important;
        padding: 1.2rem;
        margin-bottom: 1rem;
        color: white;
        width:200px !important;
    }
    ::v-deep.v-data-table > .v-data-table__wrapper > table > tbody > tr:first-child >td{
        padding-top: 1rem;
    }
    ::v-deep.v-data-table > .v-data-table__wrapper > table  tbody  tr td{
        width:200px !important;
    }
    $grey1: #eeeeee99;
    /* width */
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

    .v-data-table, .v-data-table--fixed-header, .v-data-table__wrapper {
        /* width */
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
    }
    ::v-deep.theme--light.v-data-table.v-data-table--fixed-header thead th {
        padding: 1.2rem 6rem;
    }
    ::v-deep.theme--light.v-data-table.v-data-table--fixed-header thead th:first-child {
        padding: 1.2rem
    }

</style>