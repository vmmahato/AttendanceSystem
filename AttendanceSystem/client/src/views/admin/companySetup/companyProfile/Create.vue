<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Add Company</span>
        </v-card-title>

        <v-card-text>
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
                <v-row>
                    <v-col cols="12" md="6">
                        <v-row no-gutters>
                            <v-col cols="12">
                                <v-text-field label="Company"
                                              v-model="formData.companyName"
                                              :error-messages="companyNameErrors"
                                              @input="$v.formData.companyName.$touch()"
                                              @blur="$v.formData.companyName.$touch()"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12">
                                <v-text-field label="Company Code"
                                              v-model="formData.companyCode"
                                              :error-messages="companyCodeErrors"
                                              @input="$v.formData.companyCode.$touch()"
                                              @blur="$v.formData.companyCode.$touch()"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12">
                                <v-text-field label="Address"
                                              v-model="formData.companyAddress"
                                              :error-messages="addressErrors"
                                              @input="$v.formData.companyAddress.$touch()"
                                              @blur="$v.formData.companyAddress.$touch()"
                                              maxLength="200"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12">
                                <v-text-field label="Contact Person"
                                              v-model="formData.contactPerson"
                                              :error-messages="contactPersonErrors"
                                              @input="$v.formData.contactPerson.$touch()"
                                              @blur="$v.formData.contactPerson.$touch()"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12">
                                <v-text-field label="Branch Name"
                                              v-model="formData.branchName"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12">
                                <v-text-field label="Contact Number"
                                              v-model="formData.contactNumber"
                                              :error-messages="contactNumberErrors"
                                              @input="$v.formData.contactNumber.$touch()"
                                              @blur="$v.formData.contactNumber.$touch()"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12">
                                <v-text-field label="Email"
                                              v-model="formData.email"
                                              :error-messages="emailErrors"
                                              @input="$v.formData.email.$touch()"
                                              @blur="$v.formData.email.$touch()"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12">
                                <v-text-field label="Web"
                                              v-model="formData.webSite"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12">
                                <v-text-field label="Pan"
                                              v-model="formData.panNumber"
                                              maxLength="9"
                                              :error-messages="panErrors"
                                              @input="$v.formData.panNumber.$touch()"
                                              @blur="$v.formData.panNumber.$touch()"
                                ></v-text-field>
                            </v-col>
                        </v-row>
                    </v-col>
                    <v-col cols="12" md="6">
                        <div id="preview">
                            <v-img v-if="formData.companyImage" :src="imageURL" alt="Upload Image"/>
                        </div>
                        <v-file-input @change="onFileChange"
                                      chips clearable
                                      label="Choose Image"
                                      v-model="formData.companyImage"
                                      :error-messages="companyImageErrors"
                                      @input="$v.formData.companyImage.$touch()"
                                      @blur="$v.formData.companyImage.$touch()"
                        ></v-file-input>
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
    import {email, helpers, maxLength, minLength, numeric, required} from "vuelidate/lib/validators";
    import axios from 'axios'

    const isPhone = helpers.regex('alpha', /^[0-9+\- ]*$/)
    const isTrueImage = (value) => {
        if (!value) {
            return true;
        }
        return value.type.startsWith('image');
    }
    export default {
        name: "CompanyCreate",
        computed: {
            companyNameErrors() {
                const errors = []
                if (!this.$v.formData.companyName.$dirty) return errors
                !this.$v.formData.companyName.required && errors.push('Company Name is required.')
                return errors
            },
            companyCodeErrors() {
                const errors = []
                if (!this.$v.formData.companyCode.$dirty) return errors
                !this.$v.formData.companyCode.required && errors.push('Company Code is required.')
                return errors
            },
            addressErrors() {
                const errors = []
                if (!this.$v.formData.companyAddress.$dirty) return errors
                !this.$v.formData.companyAddress.required && errors.push('Company Address is required.')
                return errors
            },
            contactPersonErrors() {
                const errors = []
                if (!this.$v.formData.contactPerson.$dirty) return errors
                !this.$v.formData.contactPerson.required && errors.push('Contact Person is required.')
                return errors
            },
            companyImageErrors() {
                const errors = []
                if (!this.$v.formData.companyImage.$dirty) return errors
                !this.$v.formData.companyImage.isTrueImage && errors.push('Image is required.')
                return errors
            },
            contactNumberErrors() {
                const errors = []
                if (!this.$v.formData.contactNumber.$dirty) return errors
                !this.$v.formData.contactNumber.required && errors.push('Contact Number is required.')
                !this.$v.formData.contactNumber.isPhone && errors.push('Contact Number must be Valid.')
                !this.$v.formData.contactNumber.maxLength && errors.push('Not a Valid Contact Number')
                !this.$v.formData.contactNumber.minLength && errors.push('Minimum Length Requirement not met')
                return errors
            },
            emailErrors() {
                const errors = []
                if (!this.$v.formData.email.$dirty) return errors
                !this.$v.formData.email.required && errors.push('Email is required.')
                !this.$v.formData.email.email && errors.push('Valid Email is required.')
                return errors
            },
            panErrors() {
                const errors = []
                if (!this.$v.formData.panNumber.$dirty) return errors
                !this.$v.formData.panNumber.required && errors.push('Pan is required.')
                !this.$v.formData.panNumber.numeric && errors.push('Pan is Invalid.')
                !this.$v.formData.panNumber.maxLength && errors.push('Pan is Invalid.')
                return errors
            },
        },
        data() {
            return {
                snackbar: {
                    data: {},
                    message: '',
                    color: '',
                    snackbar: false
                },
                formData: {},
                imageURL: '',
                submitStatus: ''
            }
        },
        methods: {
            closeForm() {
                this.$v.$reset()
                if(this.snackbar.color === 'error'){
                    this.snackbar.message = ''
                }
                this.$emit("closeDialog", this.snackbar)
            },
            async saveData() {
                this.$v.$touch()
                if (this.$v.$invalid) {
                    this.submitStatus = 'ERROR'
                } else {
                    try {
                        let form_data = new FormData();

                        for (let key in this.formData) {
                            form_data.append(key, this.formData[key]);
                        }
                        const {data} = await axios.post('CompanyProfile/CreateCompanyProfile', form_data)
                        this.snackbar.snackbar = true
                        this.snackbar.message = data.success ? 'Company Created Successfully' : data.errors.join(', ')
                        this.snackbar.color = data.success ? 'success' : 'error'
                        this.snackbar.data = data
                        this.closeForm()
                    } catch (e) {
                        console.log(e)
                        //this.closeForm()
                    }
                }
            },
            onFileChange(e) {
                this.imageURL = URL.createObjectURL(e);
            }
        },
        created() {

        },
        validations() {
            return {
                formData: {
                    companyName: {required},
                    companyCode: {required},
                    companyAddress: {required},
                    contactPerson: {required},
                    contactNumber: {
                        required,
                        isPhone,
                        maxLength: maxLength(15),
                        minLength: minLength(7)
                    },
                    email: {required, email},
                    companyImage: {
                        isTrueImage
                    },
                    panNumber: {
                        required,
                        numeric,
                        maxLength: maxLength(9)
                    },
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