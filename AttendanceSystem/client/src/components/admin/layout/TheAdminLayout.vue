<template>
    <v-app id="inspire">
        <v-app-bar
                :clipped-left="$vuetify.breakpoint.smAndUp"
                app
                color="teal"
                dark
        >
            <v-toolbar-title style="width: 280px"
                             class="ml-0 pl-1 mr-1"
            >
                <span class="hidden-xs-only">SOFTWEB Developers</span>
            </v-toolbar-title>
            <v-app-bar-nav-icon @click.stop="toggleSideBar"></v-app-bar-nav-icon>

            <v-toolbar-title
                    class="pl-4"
                    @click.stop=""
            >
               <v-icon>mdi-share-variant</v-icon>
                <span class="hidden-sm-and-down pl-1">Device</span>
            </v-toolbar-title>
            <!-- <v-text-field
                     flat
                     solo-inverted
                     hide-details
                     prepend-inner-icon="mdi-magnify"
                     label="Search"
                     class="hidden-sm-and-down"
             ></v-text-field>-->
            <v-spacer></v-spacer>
            <v-menu offset-y>
                <template v-slot:activator="{ on, attrs }">
                    <v-btn
                            color="teal"
                            depressed
                            dark
                            v-bind="attrs"
                            v-on="on"
                    >
                        <span class="pr-1">{{userData.username}}</span>
                        <v-icon>mdi-tools</v-icon>
                    </v-btn>
                </template>
                <v-list>
                    <v-list-item two-line class="border">
                        <v-list-item-avatar>
                            <img src="https://randomuser.me/api/portraits/men/81.jpg"/>
                        </v-list-item-avatar>

                        <v-list-item-content class="text-left">
                            <v-list-item-title>{{userData.name}}</v-list-item-title>
                            <v-list-item-subtitle>{{userData.role}}</v-list-item-subtitle>
                        </v-list-item-content>
                    </v-list-item>
                    <v-list-item
                            v-for="(item,index) in userMenuItem"
                            :key="index"
                            link
                            :to="item.link"
                    >
                        <v-list-item-icon>
                            <v-icon>{{'mdi-'+item.icon}}</v-icon>
                        </v-list-item-icon>
                        <v-list-item-content class="text-left">
                            <v-list-item-title>{{item.menuName}}</v-list-item-title>
                        </v-list-item-content>
                    </v-list-item>
                </v-list>
            </v-menu>
            <v-app-bar-nav-icon @click.stop="toggleRightSideBar"></v-app-bar-nav-icon>
        </v-app-bar>
        <TheSideBar :drawers="drawer" :rightDrawers="rightDrawer"></TheSideBar>

        <v-main>
                    <transition name="fade">
                        <router-view></router-view>
                    </transition>
        </v-main>

        <v-footer>
            ...
        </v-footer>
    </v-app>
</template>

<script>
    import TheSideBar from './TheSideBar'
    import {mapActions, mapGetters} from "vuex"

    export default {
        name: "AdminDashboard",
        props: {
            source: String,
        },
        computed: {
            ...mapGetters([
                'getInfo'
            ]),
        },
        components: {
            TheSideBar
        },
        data: () => ({
            dialog: false,
            drawer: true,
            rightDrawer: false,
            userMenu: false,
            userData: {
                name: '',
                role: '',
                username: ''
            },
            userMenuItem: [
                {icon: 'cog', menuName: 'Account Setting', link: ''},
                {icon: 'power', menuName: 'Log Out', link: '/logout'},
            ]
        }),
        created() {
            this.getMenuData(localStorage.getItem('userInfo'))
            this.dateLangSetter(JSON.parse(localStorage.getItem('dateLang')))
            this.getUserData()
        },
        methods: {
            ...mapActions({
                getMenuData: "userData",
                dateLangSetter:"set_lang"
            }),
            toggleSideBar() {
                this.drawer = !this.drawer
            },
            toggleRightSideBar() {
                this.rightDrawer = !this.rightDrawer
            },
            toggleUserMenu() {
                this.userMenu = !this.userMenu
            },
            getUserData() {
                let parsedData = JSON.parse(this.getInfo)
                this.userData.name = parsedData.user
                this.userData.role = parsedData.role
                this.userData.username = parsedData.userName
            }
        },
    }
</script>
<style lang="scss">
    ::v-deep.customFontSize ::v-deep.v-card__text ::v-deep.v-text-field input ,::v-deep.customFontSize ::v-deep.v-card__text ::v-deep.v-input--selection-controls input {
        font-size: 0.8em;
        text-transform: capitalize;
    }
    ::v-deep.customFontSize ::v-deep.v-card__text ::v-deep.v-text-field label, ::v-deep.customFontSize ::v-deep.v-card__text ::v-deep.v-list-item__content ::v-deep.v-list-item__title ,::v-deep.customFontSize .v-input--selection-controls label {
        font-size: 0.8em !important;
    }
</style>