<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Edit Work Shift</span>
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
                    <v-col cols="12" md="3">
                        <v-text-field label="Code"
                                      v-model="formData.shiftCode"
                                      :error-messages="shiftCodeErrors"
                                      @input="$v.formData.shiftCode.$touch()"
                                      @blur="$v.formData.shiftCode.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-text-field label="Name"
                                      v-model="formData.shiftName"
                                      :error-messages="shiftNameErrors"
                                      @input="$v.formData.shiftName.$touch()"
                                      @blur="$v.formData.shiftName.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-digital-time-picker
                                label="Shift Start"
                                v-model="formData.shiftStart"
                                prepend-icon="access_time"
                        />
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-digital-time-picker
                                label="Shift End"
                                v-model="formData.shiftEnd"
                                prepend-icon="access_time"
                        />
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-digital-time-picker
                                label="Beginning In"
                                v-model="formData.beginingIn"
                                prepend-icon="access_time"
                        />
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-digital-time-picker
                                label="Ending In"
                                v-model="formData.endingIn"
                                prepend-icon="access_time"
                        />
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-digital-time-picker
                                label="Beginning Out"
                                v-model="formData.beginingOut"
                                prepend-icon="access_time"
                        />
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-digital-time-picker
                                label="Ending Out"
                                v-model="formData.endingOut"
                                prepend-icon="access_time"
                        />
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-digital-time-picker
                                label="Late In"
                                v-model="formData.lateIn"
                                prepend-icon="access_time"
                        />
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-digital-time-picker
                                label="Leave Early"
                                v-model="formData.leaveEarly"
                                prepend-icon="access_time"
                        />
                    </v-col>
                </v-row>
                <v-card color="teal lighten-5">
                    <v-card-text v-if="formDataArray.length">
                        <v-row v-for="(item,index) in formDataArray" :key="index">
                            <v-col cols="12" md="3">
                                <v-digital-time-picker
                                        v-model="formDataArray[index].startTime"
                                        :label="`${item.name}  Start Time`"
                                        prepend-icon="access_time"
                                />
                            </v-col>
                            <v-col cols="12" md="3">
                                <v-digital-time-picker
                                        v-model="formDataArray[index].endTime"
                                        :label="`${item.name}  End Time`"
                                        prepend-icon="access_time"
                                />
                            </v-col>
                            <v-col cols="12" md="3">
                                <v-text-field v-model.number="formDataArray[index].count"
                                              type="number"
                                              :label="`${item.name}  Count`"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="3">
                                <v-text-field v-model.number="formDataArray[index].duration"
                                              :label="`${item.name}  Duration in minute`"
                                              type="number"
                                ></v-text-field>
                            </v-col>
                        </v-row>
                    </v-card-text>
                </v-card>
                <v-card flat>
                    <v-card-actions>
                        <v-combobox multiple
                                    v-model="snackbar.select"
                                    label="Add Field if Necessary"
                                    chips
                                    dense
                                    append-icon=""
                                    color="teal"
                                    deletable-chips
                                    class="tag-input"
                                    :append-outer-icon="'mdi-send'"
                                    @click:append-outer="addField"
                        >
                        </v-combobox>
                    </v-card-actions>
                </v-card>
                <v-row>
                    <v-col cols="12" md="3">
                        <v-switch v-model="formData.isMustCheckIn"
                                  label="Must Check In ?"
                                  clearable
                                  color="teal"
                        ></v-switch>
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-switch v-model="formData.isMustCheckOut"
                                  label="Must Check Out ?"
                                  clearable
                                  color="teal"
                        ></v-switch>
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-switch v-model="formData.isLateIn"
                                  label="Late In"
                                  clearable
                                  color="teal"
                        ></v-switch>
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-switch v-model="formData.isEarlyLeave"
                                  label="Early Leave"
                                  clearable
                                  color="teal"
                        ></v-switch>
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
    import {required} from "vuelidate/lib/validators"

    export default {
        name: "WorkShiftEdit",
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
            shiftNameErrors() {
                const errors = []
                if (!this.$v.formData.shiftName.$dirty) return errors
                !this.$v.formData.shiftName.required && errors.push('Shift Name is required.')
                return errors
            },
            shiftCodeErrors() {
                const errors = []
                if (!this.$v.formData.shiftCode.$dirty) return errors
                !this.$v.formData.shiftCode.required && errors.push(' Code is required.')
                return errors
            },
        },
        data() {
            return {
                snackbar: {
                    snackbar:false,
                    loopMenuStartTime: [false],
                    loopMenuEndTime: [false],
                    data: {},
                    message: '',
                    color: '',
                    select: []
                },
                formData: {
                    shiftList: []
                },
                formDataArray:[],
                tempFilteredArray:[],
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
            async saveData() {
                this.$v.$touch()
                if (this.$v.$invalid) {
                    this.submitStatus = 'ERROR'
                } else {
                    try {
                        this.formData.shiftList = []
                        this.formData.shiftList = this.formDataArray
                        const {data} = await axios.post('WorkShift/UpdateWorkShiftAsync', this.formData)
                        this.snackbar.message = data.success ? 'Shift Updated Successfully' : data.errors.join(', ')
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
                    this.formData = data.workShift
                    this.formData.shiftEnd = this.formData.shiftEndString
                    this.formData.shiftStart = this.formData.shiftStartString
                    this.formData.beginingIn = this.formData.beginingInString
                    this.formData.endingIn = this.formData.endingInString
                    this.formData.lateIn = this.formData.lateInString
                    this.formData.beginingOut = this.formData.beginingOutString
                    this.formData.beginingOut = this.formData.beginingOutString
                    this.formData.endingOut = this.formData.endingOutString
                   // this.formDataArray = []
                    this.formDataArray = data.workShiftType.map((data)=>{
                        return {
                            name:data.name,
                            startTime: data.startTimeString,
                            endTime: data.endTimeString,
                            count: data.count,
                            duration: data.duration,
                        }
                    })
                    this.snackbar.select = data.workShiftType.map(({name})=>{
                        return name
                    })
                } catch (e) {
                    console.log(e)
                }

            },
            addField() {
                if(this.snackbar.select.length){
                    this.snackbar.select.forEach(item => {
                        console.log('inside loop')
                        if (this.formDataArray.filter(e => e.name === item).length <= 0) {
                            console.log('inside if')
                            this.formDataArray.push({
                                name: item,
                                startTime: '',
                                endTime: '',
                                count: '',
                                duration: '',
                            })

                            /* for(let i=0;i<this.formDataArray.length;i++){
                                 if(!this.snackbar.select.contains(this.formDataArray[i].name)){

                                 }
                             }*/

                        }
                        this.tempFilteredArray = []
                        this.tempFilteredArray = this.formDataArray.filter((item) => {
                            return this.snackbar.select.includes(item.name)
                        })
                    })
                    this.formDataArray = this.tempFilteredArray
                }else{
                    this.formDataArray = []
                }

            }
        },
        created() {
            this.getEditData()
        },
        validations: {
            formData: {
                shiftName: {required},
                shiftCode: {required},
            }
        },
    }
</script>

<style scoped lang="scss">
    ::v-deep.tag-input span.v-chip {
        background-color: teal;
        color: #fff;
        font-size: 1em;
        padding-left: 7px;
    }
</style>