import store from '@/store'
export default [
    {
        path: '/',
        meta: {requiresAuth: true},
        name:'admin',
        component: () => import(/* webpackChunkName: "homePage" */ `@/components/admin/layout/TheAdminLayout.vue`),
        children: [
            {
                path: '',
                name: 'dashboard',
                component: () => import(/* webpackChunkName: "homePage" */ `@/components/admin/Dashboard/TheAdminDashboard.vue`),
            },
            {
                path: '/dashboard',
                component: () => import(/* webpackChunkName: "homePage" */ `@/components/admin/Dashboard/TheAdminDashboard.vue`),
            },
            {
                path: '/logout',
                name: 'logOut',
                beforeEnter: (to, from, next) => {
                    store.dispatch('logout').then(()=> next({name:'home'}))
                }
            },
            {
                path: '/fiscalYear',
                name: 'fiscalYear',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/companySetup/fiscalYear/Grid.vue`),
            },
            {
                path: '/companyProfile',
                name: 'companyProfile',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/companySetup/companyProfile/Grid.vue`),
            },
            {
                path: '/department',
                name: 'department',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/companySetup/department/Grid.vue`),
            },
            {
                path: '/designation',
                name: 'designation',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/companySetup/designation/Grid.vue`),
            },
            {
                path: '/workShifts',
                name: 'workShifts',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/companySetup/workShifts/Grid.vue`),
            },
            {
                path: '/deviceManager',
                name: 'deviceManager',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/companySetup/deviceManager/Grid.vue`),
            },
            {
                path: '/companyProfile',
                name: 'companyProfile',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/companySetup/fiscalYear/Grid.vue`),
            },
            {
                path: '/employee',
                name: 'employee',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/officeManagement/employee/Grid.vue`),
            },
            {
                path: '/dynamicRoster',
                name: 'dynamicRoster',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/officeManagement/dynamicRoster/Grid.vue`),
            },
            {
                path: '/weeklyRoster',
                name: 'weeklyRoster',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/officeManagement/weeklyRoster/Grid.vue`),
            },
            {
                path: '/fixedRoster',
                name: 'fixedRoster',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/officeManagement/fixedRoster/Grid.vue`),
            },
            {
                path: '/officeVisit',
                name: 'officeVisit',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/officeManagement/officeVisit/Grid.vue`),
            },
            {
                path: '/officeApproval',
                name: 'officeApproval',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/officeManagement/officeApproval/Grid.vue`),
            },
            {
                path: '/kaj',
                name: 'kaj',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/officeManagement/kaj/Grid.vue`),
            },
            {
                path: '/kajApproval',
                name: 'kajApproval',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/officeManagement/kajApproval/Grid.vue`),
            },
            {
                path: '/manualPunch',
                name: 'manualPunch',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/officeManagement/manualPunch/Grid.vue`),
            },
            {
                path: '/holiday',
                name: 'holiday',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/officeManagement/holiday/Grid.vue`),
            },
            {
                path: '/leaveApplication',
                name: 'leaveApplication',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/leaveManagement/leaveApplication/Grid.vue`),
            },
            {
                path: '/leaveApproval',
                name: 'leaveApproval',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/leaveManagement/leaveApproval/Grid.vue`),
            },
            {
                path: '/leaveQuota',
                name: 'leaveQuota',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/leaveManagement/leaveQuota/Grid.vue`),
            },
            {
                path: '/leaveOpeningBalance',
                name: 'leaveOpeningBalance',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/leaveManagement/leaveOpeningBalance/Grid.vue`),
            },
            {
                path: '/leaveSettlement',
                name: 'leaveSettlement',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/leaveManagement/leaveSettlement/Grid.vue`),
            },
            {
                path: '/leaveSetup',
                name: 'leaveSetup',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/leaveManagement/leaveSetup/Grid.vue`),
            },
            {
                path: '/userManagement',
                name: 'userManagement',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/userManagement/Grid.vue`),
            },
            {
                path: '/allowances',
                name: 'allowances',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/payroll/allowances/Grid.vue`),
            },
            {
                path: '/deductions',
                name: 'deductions',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/payroll/deductions/Grid.vue`),
            },
            {
                path: '/quota',
                name: 'quota',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/companySetup/fiscalYear/Grid.vue`),
            },
            {
                path: '/payslip',
                name: 'payslip',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/companySetup/fiscalYear/Grid.vue`),
            },
            {
                path: '/attendanceReport',
                name: 'attendanceReport',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/report/attendanceReport/Grid.vue`),
            },
            {
                path: '/leaveReport',
                name: 'leaveReport',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/companySetup/fiscalYear/Grid.vue`),
            },
            {
                path: '/payslipReport',
                name: 'payslipReport',
                component: () => import(/* webpackChunkName: "homePage" */ `@/views/admin/companySetup/fiscalYear/Grid.vue`),
            },
            {
                path: '/test',
                name: 'test',
                component: () => import(/* webpackChunkName: "testPage" */ `@/views/admin/test/Grid.vue`),
            },
        ]
    },
    {
        path: '/systemLogin',
        meta: {requiresAuth: false},
        name: 'home',
        component: () => import(/* webpackChunkName: "homePage" */ `@/views/public/Home.vue`),
    },
]
