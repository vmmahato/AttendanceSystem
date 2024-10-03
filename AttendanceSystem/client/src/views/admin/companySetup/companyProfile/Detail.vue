<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Company Profile</span>
        </v-card-title>

        <v-card-text>
            <v-container>
                <v-row>
                    <v-col cols="12" md="6">
                        <v-text-field label="Company" readonly
                                      v-model="formData.companyName"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field label="Company Code" readonly
                                      v-model="formData.companyCode"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field label="Address" readonly
                                      v-model="formData.companyAddress"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field label="Contact Person" readonly
                                      v-model="formData.contactPerson"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field label="Branch Name" readonly
                                      v-model="formData.branchName"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field label="Contact Number" readonly
                                      v-model="formData.contactNumber"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field label="Email" readonly
                                      v-model="formData.email"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field label="Web" readonly
                                      v-model="formData.webSite"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field label="Pan" readonly
                                      v-model="formData.panNumber"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <div id="preview">
                            <v-img v-if="formData.companyImage" :src="imageURL" alt="Upload Image"/>
                        </div>
                        <v-file-input
                                readonly
                                @change="onFileChange"
                                chips clearable
                                label="Choose Image"
                                v-model="formData.companyImage"
                        ></v-file-input>
                    </v-col>
                </v-row>
            </v-container>
        </v-card-text>

        <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="red darken-1" text @click="closeForm">Close</v-btn>
        </v-card-actions>
    </v-card>
</template>

<script>
    import axios from 'axios'

    export default {
        name: "CompanyDetail",
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
                imageURL: ''
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
            this.getEditData()
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