<template>
    <v-app id="inspire">
        <div class="home">
            <v-card class="center-div">
                <v-card flat class="item-left pa-5">
                    <v-card-title class=" text-h4">Login</v-card-title>
                    <v-card-subtitle class="subtitle-1">Sign in to your account</v-card-subtitle>
                    <v-card-text>
                        <v-text-field
                                outlined
                                label="Username"
                                prepend-inner-icon="mdi-account"
                                v-model="signInData.username"
                        ></v-text-field>
                        <v-text-field
                                outlined
                                label="Password"
                                prepend-inner-icon="mdi-lock"
                                type="password"
                                v-model="signInData.password"
                        ></v-text-field>
                    </v-card-text>
                    <v-card-actions>
                        <v-btn color="light-blue" dark @click="submit" class="text-capitalize">Login</v-btn>
                        <v-spacer></v-spacer>
                        <router-link to="/" class="no-text-decoration pa-2">Forgot Password?</router-link>
                    </v-card-actions>
                    <!--<div class="grid-button-container">
                    </div>-->
                </v-card>
                <v-card dark color="teal" class="item-right pa-5 text-right">
                    <v-card-title class=" text-md-h4">Attendance and Payroll System</v-card-title>
                    <v-card-text class="subtitle-1">
                        Attendance and Payroll System is a tool for tracking employee time attendance, absenteeism,
                        leave/holidays, official visits
                        and payroll management and well managed reports for efficient management.
                    </v-card-text>
                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn  color="white" class="teal--text text-capitalize">View Manual</v-btn>
                    </v-card-actions>
                </v-card>
            </v-card>
        </div>
    </v-app>
</template>

<script>
    import {mapActions, mapGetters} from "vuex";

    export default {
        name: 'Home',
        components: {},
        data() {
            return {
                signInData: {
                    username: '',
                    password: ''
                }
            }
        },
        methods: {
            ...mapActions({
                signIn: "signIn"
            }),
            submit() {
                let formdata = JSON.stringify({
                    UserName: this.signInData.username,
                    password: this.signInData.password
                });
                this.signIn(formdata)
            },
            /*checkAuthenticated(){
                if(this.authenticated){
                    this.$router.push({
                        name:'adminDashboard'
                    })
                }
                else{
                    localStorage.clear()
                }
            }*/
        },
        computed: {
            ...mapGetters({
                authenticated: "authenticated"
            }),
        },
        mounted() {
            //this.checkAuthenticated()
            //localStorage.clear()
        }
    }
</script>
<style lang="scss" scoped>
    .home {
        height: 100vh;
        display: grid;
        align-items: center;
        justify-content: center;
        text-align: left;

        .center-div {
            max-width: 900px;
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
        }

        .grid-button-container {
            display: grid;
            grid-template-columns: 1fr 1fr;
            grid-gap: 14px;
        }
    }

    .no-text-decoration {
        text-decoration: none;
    }
    .text-teal{
        color: teal;
    }
</style>
