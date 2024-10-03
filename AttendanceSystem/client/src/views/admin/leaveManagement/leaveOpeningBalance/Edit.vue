<template>
    <v-card v-if="formData.length">
        <!-- <v-card-title>
             <span class="headline">Leave Opening Balance</span>
         </v-card-title>-->

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
                <v-simple-table fixed-header height="500px">
                    <template v-slot:default>
                        <thead>
                        <tr>
                            <th class="text-left">Employee</th>
                            <th class="text-left" v-for="(code,index) in headerData" :key="'header'+index">{{ code.id }}
                            </th>
                        </tr>
                        </thead>
                        <tbody>
                        <tr v-for="(item,index) in formData" :key="index">
                            <td>{{item.name}}</td>
                            <td v-for="(listCode,count) in item.list" :key="count+'listCode'+index">
                                <v-text-field
                                        type="number"
                                        v-model.number="listCode.value" :disabled="!listCode.show"
                                        min="0"
                                ></v-text-field>
                            </td>
                        </tr>
                        </tbody>
                    </template>
                </v-simple-table>
            </v-container>
        </v-card-text>

        <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="teal darken-1" text @click="saveData">Save</v-btn>
        </v-card-actions>
    </v-card>
</template>

<script>
    import axios from 'axios'

    export default {
        name: "LeaveQuotaEdit",
        props: {
            getItemUrl: {
                type: String,
                required: true
            },
            employeeID: {
                type: Array,
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
                    snackbar:false
                },
                formData: [],
                headerData: [],
                submitStatus: ''
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
                 this.formData.forEach((item) => {
                    item.list.forEach(listItem => {
                        if (!listItem.value) {
                            listItem.value = 0
                        }
                    })
                })
                let formSubmitData = {
                    employeeList: this.formData
                }
                console.log('formSubmitData :', formSubmitData)
                try {
                    const {data} = await axios.post('LeaveOpeningBalance/UpdateLeaveOpeningBalanceAsync', formSubmitData)
                    this.snackbar.message = data.success ? 'Data Updated Successfully' : data.errors.join(', ')
                    this.snackbar.color = data.success ? 'success' : 'error'
                    this.snackbar.data = data
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
                try {
                        const {data} = await axios.post(this.ItemDetailUrl, {
                            employeeIDs: this.employeeID
                        })
                        this.formData = data.result
                        this.headerData = data.codeList
                } catch (e) {
                    console.log(e)
                }

            }
        },
        created() {
            this.getEditData()
        },
    }
</script>

<style scoped>

</style>