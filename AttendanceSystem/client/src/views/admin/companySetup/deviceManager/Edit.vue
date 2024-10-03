<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Edit Device</span>
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
                        <v-text-field label="Device"
                                      v-model="formData.deviceName"
                                      :error-messages="deviceNameErrors"
                                      @input="$v.formData.deviceName.$touch()"
                                      @blur="$v.formData.deviceName.$touch()"></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-autocomplete v-model="formData.branch"
                                        :items="branchList"
                                        item-value="value"
                                        item-text="value"
                                        :error-messages="branchErrors"
                                        @input="$v.formData.branch.$touch()"
                                        @blur="$v.formData.branch.$touch()"
                                        label="Branch"
                                        clearable></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field label="Device Number"
                                      v-model.number="formData.deviceNumber"
                                      :error-messages="deviceNumberErrors"
                                      @input="$v.formData.deviceNumber.$touch()"
                                      @blur="$v.formData.deviceNumber.$touch()"></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-autocomplete v-model="formData.deviceModelID"
                                        :items="deviceModelList"
                                        item-value="id"
                                        item-text="value"
                                        :error-messages="deviceModelIDErrors"
                                        @input="$v.formData.deviceModelID.$touch()"
                                        @blur="$v.formData.deviceModelID.$touch()"
                                        label="Device Model"></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field label="IP Address"
                                      v-model="formData.ipAddress"
                                      :error-messages="ipAddressErrors"
                                      @input="$v.formData.ipAddress.$touch()"
                                      @blur="$v.formData.ipAddress.$touch()"></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field label="Communication Port"
                                      v-model.number="formData.communicationPort"
                                      type="number"
                                      :error-messages="communicationPortErrors"
                                      @input="$v.formData.communicationPort.$touch()"
                                      @blur="$v.formData.communicationPort.$touch()"></v-text-field>
                    </v-col>
                    <!--<v-col cols="12" md="6">
        <v-text-field label="Terminal IP"
                      v-model="formData.terminalIP"
                      :error-messages="terminalIPErrors"
                      @input="$v.formData.terminalIP.$touch()"
                      @blur="$v.formData.terminalIP.$touch()"
        ></v-text-field>
    </v-col>-->
                    <v-col cols="12" md="6">
                        <v-text-field label="serialNumber"
                                      v-model="formData.serialNumber"
                                      :error-messages="serialNumberErrors"
                                      @input="$v.formData.serialNumber.$touch()"
                                      @blur="$v.formData.serialNumber.$touch()"></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field label="Communication Password"
                                      v-model="formData.communicationPassword"
                                      :error-messages="communicationPasswordErrors"
                                      @input="$v.formData.communicationPassword.$touch()"
                                      @blur="$v.formData.communicationPassword.$touch()"></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field label="Communication Mode"
                                      v-model="formData.communicationMode"
                                      :error-messages="communicationModeErrors"
                                      @input="$v.formData.communicationMode.$touch()"
                                      @blur="$v.formData.communicationMode.$touch()"></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-switch v-model="formData.isActive"
                                  label="Is Active?"
                                  clearable
                                  color="teal"></v-switch>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-switch v-model="formData.isRegister"
                                  label="Is Registered?"
                                  clearable
                                  color="teal"></v-switch>
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
    import axios from 'axios'
    import {ipAddress, required} from "vuelidate/lib/validators"

    export default {
        name: "FiscalYearEdit",
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
            deviceNameErrors() {
                const errors = []
                if (!this.$v.formData.deviceName.$dirty) return errors
                !this.$v.formData.deviceName.required && errors.push('Device Name is required.')
                return errors
            },
            branchErrors() {
                const errors = []
                if (!this.$v.formData.branch.$dirty) return errors
                !this.$v.formData.branch.required && errors.push('Branch is required.')
                return errors
            },
            deviceNumberErrors() {
                const errors = []
                if (!this.$v.formData.deviceNumber.$dirty) return errors
                !this.$v.formData.deviceNumber.required && errors.push('Device Number is required.')
                return errors
            },
            deviceModelIDErrors() {
                const errors = []
                if (!this.$v.formData.deviceModelID.$dirty) return errors
                !this.$v.formData.deviceModelID.required && errors.push('Device Model is required.')
                return errors
            },
            ipAddressErrors() {
                const errors = []
                if (!this.$v.formData.ipAddress.$dirty) return errors
                !this.$v.formData.ipAddress.required && errors.push('IP is required.')
                !this.$v.formData.ipAddress.ipAddress && errors.push('Valid IP is required.')
                return errors
            },
            communicationPortErrors() {
                const errors = []
                if (!this.$v.formData.communicationPort.$dirty) return errors
                !this.$v.formData.communicationPort.required && errors.push('Communication Port is required.')
                return errors
            },
            //terminalIPErrors() {
            //    const errors = []
            //    if (!this.$v.formData.terminalIP.$dirty) return errors
            //    !this.$v.formData.terminalIP.required && errors.push('Terminal IP is required.')
            //    !this.$v.formData.ipAddress.ipAddress && errors.push('Valid IP is required.')
            //    return errors
            //},
            communicationPasswordErrors() {
                const errors = []
                if (!this.$v.formData.communicationPassword.$dirty) return errors
                !this.$v.formData.communicationPassword.required && errors.push('Communication Password is required.')
                return errors
            },
            communicationModeErrors() {
                const errors = []
                if (!this.$v.formData.communicationMode.$dirty) return errors
                !this.$v.formData.communicationMode.required && errors.push('Communication Mode is required.')
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
                branchList: [],
                deviceModelList: [],
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
            async getDeviceModelList() {
                let {data} = await axios.get('Device/DDLDeviceModelList')
                this.deviceModelList = data
            },
            async saveData() {
                this.$v.$touch()
                if (this.$v.$invalid) {
                    this.submitStatus = 'ERROR'
                } else {
                    try {
                        const {data} = await axios.post('Device/UpdateDevice', this.formData)
                        this.snackbar.message = data.success ? 'Device Updated Successfully' : data.errors.join(', ')
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
                    console.log(e)
                }

            },
            getFormData(){
                this.getBranchList()
                this.getDeviceModelList()
            }
        },
        created() {
            this.getFormData()
            this.getEditData()
        },
        validations: {
            formData: {
                deviceName: {required},
                deviceNumber: {required},
                deviceModelID: {required},
                ipAddress: {required, ipAddress},
                communicationPort: {required},
                communicationPassword: {required},
                communicationMode: {required},
                branch: {required},
            }
        },
    }
</script>

<style scoped>

</style>