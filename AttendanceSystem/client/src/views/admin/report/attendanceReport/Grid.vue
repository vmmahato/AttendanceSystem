<template>
    <v-container>
        <v-breadcrumbs :items="breadcrumb"></v-breadcrumbs>
        <v-tabs
                v-model="snackbar.tab"
                background-color="transparent"
                color="teal"
                right
        >
            <v-tab
                    v-for="item in tabItems"
                    :key="item.id"
            >
                {{ item.value }}
            </v-tab>
        </v-tabs>
        <v-tabs-items v-model="snackbar.tab">
            <v-tab-item
                    v-for="item in tabItems"
                    :key="item.id"
            >
                <v-card color="basil" flat>
                   <v-card-text>
                       <MonthlyReport v-if="item.id===2"></MonthlyReport>
                       <DailyReport v-else></DailyReport>
                   </v-card-text>
                </v-card>
            </v-tab-item>
        </v-tabs-items>
    </v-container>
</template>

<script>
    import {mapGetters} from "vuex"
    import DailyReport from './dailyReport/Grid'
    import MonthlyReport from './monthlyReport/Grid'

    export default {
        name: "CompanyGrid",
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
                        text: 'Attendance Report',
                        to: `${this.getSystemUserData.dashBoard}`
                    }
                ]
            },
            deleteUrl() {
                return this.snackbar.deleteUrl
            },
            deleteItemTitle() {
                return this.snackbar.deleteItemTitle
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
                    detailComponent: false,
                    getItemUrl: '',
                    deleteUrl: '',
                    deleteItemTitle: '',
                    tab:null
                },
                tabItems:[
                    {
                        id:1,
                        value:'Daily Attendance Report'
                    },
                    {
                        id:2,
                        value:'Monthly Attendance Report'
                    },
                ]
            }
        },
        methods: {

        },
        created() {

        },
        watch: {

        },
        components: {
            DailyReport,
            MonthlyReport
        }
    }
</script>

<style scoped lang="scss">
.borderClass{
    border-top: 4px solid lightseagreen;
}
</style>