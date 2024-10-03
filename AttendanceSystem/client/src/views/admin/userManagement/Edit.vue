<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Edit User Role</span>
        </v-card-title>

        <v-card-text>
            <v-container>
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
                <v-row>
                    <v-col cols="12" md="6" >
                        <v-autocomplete v-model="formData.branchID"
                                        :items="branchList"
                                        item-value="id"
                                        item-text="value"
                                        :error-messages="branchIDErrors"
                                        @input="$v.formData.branchID.$touch()"
                                        @blur="$v.formData.branchID.$touch()"
                                        label="Branch"
                                        clearable
                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="6" >
                        <v-autocomplete v-model="formData.employeeID"
                                        :items="employeeList"
                                        item-value="id"
                                        item-text="value"
                                        :error-messages="employeeIDErrors"
                                        @input="$v.formData.employeeID.$touch()"
                                        @blur="$v.formData.employeeID.$touch()"
                                        label="Employee"
                                        clearable

                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="6" >
                        <v-autocomplete v-model="formData.roleID"
                                        :items="roleList"
                                        item-text="value"
                                        item-value="id"
                                        :error-messages="roleIDErrors"
                                        @input="$v.formData.roleID.$touch()"
                                        @blur="$v.formData.roleID.$touch()"
                                        label="Role"
                                        clearable
                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-switch v-model="snackbar.changeUserCredentials"
                                  label="Change User Name and Password?"
                                  clearable
                                  color="teal"
                        ></v-switch>
                    </v-col>
                </v-row>
                <v-row v-if="isChangeUserCredential">
                    <v-col cols="12" md="6" >
                        <v-text-field label="Username"
                                      v-model="formData.userName"
                                      :error-messages="userNameErrors"
                                      @input="$v.formData.userName.$touch()"
                                      @blur="$v.formData.userName.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6" >
                        <v-text-field label="Password"
                                      v-model="formData.password"
                                      :error-messages="passwordErrors"
                                      type="password"
                                      @input="$v.formData.password.$touch()"
                                      @blur="$v.formData.password.$touch()"

                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6" >
                        <v-text-field label="Password Confirmation"
                                      v-model="formData.confirmPassword"
                                      :error-messages="confirmPasswordErrors"
                                      type="password"
                                      @input="$v.formData.confirmPassword.$touch()"
                                      @blur="$v.formData.confirmPassword.$touch()"

                        ></v-text-field>
                    </v-col>
                </v-row>
            </v-container>
        </v-card-text>

        <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="red darken-1" text @click="closeForm">Cancel</v-btn>
            <v-btn color="teal darken-1" text @click="saveData">Save</v-btn>
        </v-card-actions>
    </v-card>
</template>

<script>
    import {required, sameAs, requiredIf} from "vuelidate/lib/validators";
    import axios from 'axios'

    export default {
        name: "RolesEdit",
        props: {
            getItemUrl: {
                type: String,
                required: true
            }
        },
        computed: {
            ItemDetailUrl() {
                return this.getItemUrl
            },
            isChangeUserCredential(){
                return this.snackbar.changeUserCredentials
            },
            branchIDErrors() {
                const errors = []
                if (!this.$v.formData.branchID.$dirty) return errors
                !this.$v.formData.branchID.required && errors.push('Branch is required.')
                return errors
            },
            employeeIDErrors() {
                const errors = []
                if (!this.$v.formData.employeeID.$dirty) return errors
                !this.$v.formData.employeeID.required && errors.push('Employee Name is required.')
                return errors
            },
            roleIDErrors() {
                const errors = []
                if (!this.$v.formData.roleID.$dirty) return errors
                !this.$v.formData.roleID.required && errors.push('Role is required.')
                return errors
            },
            userNameErrors() {
                const errors = []
                if (!this.$v.formData.userName.$dirty) return errors
                !this.$v.formData.userName.required && errors.push('Username is required.')
                return errors
            },
            passwordErrors() {
                const errors = []
                if (!this.$v.formData.password.$dirty) return errors
                !this.$v.formData.password.required && errors.push('Password is required.')
                return errors
            },
            confirmPasswordErrors() {
                const errors = []
                if (!this.$v.formData.confirmPassword.$dirty) return errors
                !this.$v.formData.confirmPassword.required && errors.push('Password Confirmation is required.')
                !this.$v.formData.confirmPassword.sameAsPassword && errors.push('Passwords do not match')
                return errors
            },
        },
        data() {
            return {
                snackbar: {
                    data: {},
                    message: '',
                    color: '',
                    snackbar:false,
                    dateMenu1: null,
                    dateMenu2: null,
                    changeUserCredentials:false
                },
                formData: {},
                branchList: [],
                employeeList: [],
                roleList: [],
                imageURL: '',
                submitStatus: ''
            }
        },
        methods: {
            closeForm() {
                if(this.snackbar.color === 'error'){
                    this.snackbar.message = ''
                }
                this.$emit("closeDialog", this.snackbar)
                this.$v.$reset()
            },
            async getBranchList() {
                let {data} = await axios.get('CompanyProfile/DDLCompanyList')
                this.branchList = data
            },
            async getRolesList() {
                let {data} = await axios.get('UserRoles/DDLRolesAsync')
                this.roleList = data
            },
            async getUserList() {
                const {data} = await axios.get('Employee/DDLEmployeeList')
                this.employeeList = data
            },
            getFormData() {
                this.getBranchList()
                this.getUserList()
                this.getRolesList()
            },
            async saveData() {
                this.$v.$touch()
                if (this.$v.$invalid) {
                    this.submitStatus = 'ERROR'
                } else {
                    try {
                        const {data} = await axios.post('UserRoles/UpdateUserRolesAsync', this.formData)
                        this.snackbar.message = data.success ? 'Roles Updated Successfully' : data.errors.join(', ')
                        this.snackbar.color = data.success ? 'success' : 'error'
                        this.snackbar.data = data
                        if (data.success) {
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
                }

            },
            async getEditData() {
                try {
                    const {data} = await axios.get(this.ItemDetailUrl)
                    this.formData = data
                } catch (e) {
                    this.snackbar.message = e
                    this.snackbar.color = 'error'
                    this.snackbar.data = e
                    this.snackbar.snackbar = true
                }
            },
            onFileChange(e) {
                this.imageURL = URL.createObjectURL(e);
            }
        },
        created() {
            this.getFormData()
            this.getEditData()
        },
        validations() {
           if(this.isChangeUserCredential){
               return {
                   formData: {
                       roleID: {
                           required
                       },
                       userName: {
                           required
                       },
                       password: {
                           required
                       },
                       confirmPassword: {
                           required,
                           sameAsPassword: sameAs('password')
                       },
                       branchID: {required},
                       employeeID: {required}

                   }
               }

           }
           else{
               return {
                   formData: {
                       roleID: {
                           required
                       },
                       userName: {
                           required
                       },
                       password: {
                           required: requiredIf(function (snackbar) {
                               return snackbar.changeUserCredentials
                           })
                       },
                       confirmPassword: {
                           required: requiredIf(function (snackbar) {
                               return snackbar.changeUserCredentials
                           }),
                       },
                       branchID: {required},
                       employeeID: {required}

                   }
               }
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
</style>