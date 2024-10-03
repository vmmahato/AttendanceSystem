<template>
    <div>
        <v-navigation-drawer
                v-if="sideDrawer"
                :clipped="$vuetify.breakpoint.lgAndUp"
                :width="300"
                app
                permanent
        >
            <v-list active-class="customActiveClass">
                <v-list-item active-class="customActiveClass" two-line class="border">
                    <v-list-item-avatar>
                        <img src="https://randomuser.me/api/portraits/men/81.jpg" />
                    </v-list-item-avatar>

                    <v-list-item-content class="text-left">
                        <v-list-item-title>{{userData.name}}</v-list-item-title>
                        <v-list-item-subtitle>{{userData.role}}</v-list-item-subtitle>
                    </v-list-item-content>
                </v-list-item>
                <v-list-item
                        active-class="customActiveClass"
                        link
                        :to="userData.userDashboard"
                >
                    <v-list-item-icon>
                        <v-icon>mdi-view-dashboard</v-icon>
                    </v-list-item-icon>
                    <v-list-item-content class="text-left">
                        <v-list-item-title>Dashboard</v-list-item-title>
                    </v-list-item-content>
                </v-list-item>
                <template v-for="(item,index) in sideMenus">
                    <v-list-item
                            v-if="!item.menuList.length"
                            :key="index"
                            link
                            :to="item.parentMenu.link"
                            active-class="customActiveClass"
                    >
                        <v-list-item-icon>
                            <v-icon>{{'mdi-'+item.parentMenu.icon}}</v-icon>
                        </v-list-item-icon>
                        <v-list-item-content class="text-left">
                            <v-list-item-title>{{item.parentMenu.menuName}}</v-list-item-title>
                        </v-list-item-content>
                    </v-list-item>
                    <v-list-group
                            :key="index"
                            v-else
                            :prepend-icon="item.parentMenu?'mdi-'+item.parentMenu.icon:''"
                            append-icon="mdi-menu-down"
                            color="teal"
                    >
                        <template v-slot:activator>
                            <v-list-item-content class="text-left">
                                <v-list-item-title v-text="item.parentMenu.menuName"></v-list-item-title>
                            </v-list-item-content>
                        </template>

                        <v-list-item
                                v-for="subItem in item.menuList"
                                :key="subItem.menuName"
                                link
                                :to="subItem.link"
                                :class="subItem.link === $route.path ? 'customActiveClass' : 'teal lighten-4'"
                                class="pl-9"
                                active-class="customActiveClass"
                        >
                            <v-list-item-action v-if="subItem.icon">
                                <v-icon>{{ 'mdi-'+subItem.icon }}</v-icon>
                            </v-list-item-action>
                            <v-list-item-content class="text-left">
                                <v-list-item-title>
                                    {{subItem.menuName}}
                                </v-list-item-title>
                            </v-list-item-content>
                        </v-list-item>
                    </v-list-group>
                </template>

                <!--
                <v-list-item
                        v-for="subItem in item.items"
                        :key="subItem.title"

                >
                    <v-list-item-content>
                        <v-list-item-title v-text="subItem.title"></v-list-item-title>
                    </v-list-item-content>
                </v-list-item>-->
            </v-list>
        </v-navigation-drawer>
        <v-navigation-drawer
                absolute
                right
                key="rightBar"
                color="teal lighten-4"
                v-if="secondaryDrawer"
                class="pb-5 pb-15"
        >
            <v-list active-class="customActiveClass" class="py-5 mt-15">
                <v-list-item active-class="customActiveClass" two-line class="border mt-15">
                    <v-switch v-model="dateLang"
                              @change="formatChange"
                              label="Date Nepali?"
                              class="teal--text"
                              color="teal">
                    </v-switch>
                </v-list-item>
            </v-list>
        </v-navigation-drawer>
    </div>
</template>

<script>
    import {mapGetters, mapActions} from "vuex"

    export default {
        name: "SideBar",
        props: {
            rightDrawers:{
                type:Boolean,
                required: true
            },
            drawers:{
                type:Boolean,
                required: true
            },
        },
        computed: {
            ...mapGetters([
                'getInfo'
            ]),
            sideDrawer() {
                return this.drawers
            },
           secondaryDrawer() {
                return this.rightDrawers
            }
        },
        data: () => ({
            dialog: false,
            drawer: true,
            dateLang:JSON.parse(localStorage.getItem('dateLang')),
            items: [
                {
                    action: 'mdi-account-edit',
                    title: 'Attractions',
                    items: [
                        {title: 'List Item'},
                    ],
                },
                {
                    action: 'restaurant',
                    title: 'Dining',
                    active: true,
                    items: [
                        {title: 'Breakfast & brunch'},
                        {title: 'New American'},
                        {title: 'Sushi'},
                    ],
                },
                {
                    action: 'school',
                    title: 'Education',
                    items: [
                        {title: 'List Item'},
                    ],
                },
                {
                    action: 'directions_run',
                    title: 'Family',
                    items: [
                        {title: 'List Item'},
                    ],
                },
                {
                    action: 'healing',
                    title: 'Health',
                    items: [
                        {title: 'List Item'},
                    ],
                },
                {
                    action: 'content_cut',
                    title: 'Office',
                    items: [
                        {title: 'List Item'},
                    ],
                },
                {
                    action: 'local_offer',
                    title: 'Promotions',
                    items: [
                        {title: 'List Item'},
                    ],
                },
            ],
            sideMenus: [],
            userData:{
                name:'',
                role:'',
                userDashboard:''
            }
            /* items: [
                 { icon: 'mdi-contacts', text: 'Contacts' },
                 { icon: 'mdi-history', text: 'Frequently contacted' },
                 { icon: 'mdi-content-copy', text: 'Duplicates' },
                 {
                     icon: 'mdi-chevron-up',
                     'icon-alt': 'mdi-chevron-down',
                     text: 'Labels',
                     model: true,
                     children: [
                         { icon: 'mdi-plus', text: 'Create label' },
                     ],
                 },
                 {
                     icon: 'mdi-chevron-up',
                     'icon-alt': 'mdi-chevron-down',
                     text: 'More',
                     model: false,
                     children: [
                         { text: 'Import' },
                         { text: 'Export' },
                         { text: 'Print' },
                         { text: 'Undo changes' },
                         { text: 'Other contacts' },
                     ],
                 },
                 { icon: 'mdi-cog', text: 'Settings' },
                 { icon: 'mdi-message', text: 'Send feedback' },
                 { icon: 'mdi-help-circle', text: 'Help' },
                 { icon: 'mdi-cellphone-link', text: 'App downloads' },
                 { icon: 'mdi-keyboard', text: 'Go to the old version' },
             ],*/
        }),
        created() {
            this.getMenu()
            this.getUserData()
        },
        methods: {
            ...mapActions({
                formatChange:"set_lang"
            }),
            toggleSideBar() {
                this.drawer = !this.drawer
            },
            getMenu() {
                let parsedData = JSON.parse(this.getInfo)
                this.sideMenus = parsedData.menuList
            },
            getUserData(){
                let parsedData = JSON.parse(this.getInfo)
                this.userData.name = parsedData.user
                this.userData.role = parsedData.role
                this.userData.userDashboard = parsedData.dashBoard
            }
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
    ::v-deep.customActiveClass{
        background: #009688 !important;
        color: white !important;
    }
    .v-navigation-drawer--right{
        z-index: 4
    }
</style>