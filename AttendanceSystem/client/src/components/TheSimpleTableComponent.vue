<template>
    <v-card depressed :class="tableClass">
        <v-card-title class="body-1 pb-1">{{tableTitle}}</v-card-title>
        <v-divider class="mx-4 ma-0 pa-0"></v-divider>
        <v-card-text class="subtitle-1 pa-0 ma-0">
            <v-simple-table :fixed-header="tableHeaderType"
                            :height="tableHeight"
            >
                <template v-slot:default>
                    <thead>
                    <tr>
                        <th class="text-left teal--text"
                            v-for="(header,index) in tableHeader" :key="index+CustomKey +'header'"
                        >
                            {{header.text}}
                        </th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr v-for="(item,counter) in tableData" :key="counter+CustomKey +'body'" class="text-left">
                        <td v-for="(column,key) in tableHeader" :key="CustomKey+key+'column'">
                        <span v-if="column.value==='fromDate'">{{ item[column.value] | moment(" MMMM Do YYYY") }}</span>
                        <span v-else-if="column.value==='toDate'">{{ item[column.value] | moment(" MMMM Do YYYY") }}</span>
                        <span v-else-if="column.value==='puntchdate'">{{ item[column.value] | moment("LT") }}</span>
                            <span v-else>{{ item[column.value] }}</span>
                        </td>
                    </tr>
                    </tbody>
                </template>
            </v-simple-table>
        </v-card-text>
    </v-card>
</template>

<script>
    import axios from 'axios'

    export default {
        name: "TheTableComponent",
        props: {
            classes: {
                type: String
            },
            title: {
                required: true,
                type: String
            },
            height: {
                type: String
            },
            header: {
                type: Array,
                required: true,
            },
            headerType: {
                type: String,
            },
            items: {
                type: Array,
                required: true,
            },
            tableKey: {
                type: String,
                required: true,
            },
        },
        computed: {
            url() {
                return this.deleteUrl;
            },
            tableHeight() {
                return this.height
            },
            tableHeaderType() {
                return this.headerType
            },
            tableTitle() {
                return this.title
            },
            tableClass() {
                return this.classes
            },
            tableData() {
                return this.items.filter((item) => {
                    return this.tableHeader.find((index) => item[index.value])
                });
            },
            tableHeader() {
                return this.header
            },
            CustomKey() {
                return this.tableKey
            }
        },
        data() {
            return {
                snackbar: {
                    data: {},
                    message: "",
                    color: ""
                }
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
                try {
                    const {data} = await axios.post(this.url, {})
                    this.snackbar.data = data
                    this.snackbar.message = data.success ? 'Data Deleted Successfully' : data.errors
                    this.snackbar.color = data.success ? "success" : 'error'
                    this.closeForm()
                } catch (e) {
                    console.log(e)
                }
            }
        },
    }
</script>

<style scoped lang="scss">
    $grey1: #eeeeee99;
    /* width */
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

    .v-data-table, .v-data-table--fixed-header, .v-data-table__wrapper {
        /* width */
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
    }

    .onLeaveCard {
        border-top: 4px solid teal;
    }

    .absentCard {
        border-top: 4px solid darkred;
    }

    .upComingCard {
        border-top: 4px solid rebeccapurple;
    }

    .officialVisitsCard {
        border-top: 4px solid lightseagreen;
    }

    .deviceLogCard {
        border-top: 4px solid cyan;
    }

    .activityLogCard {
        border-top: 4px solid indigo;
    }

    .devicesCard {
        border-top: 4px solid purple;
    }
</style>