var report = [
    {
        arrayData: null,
        canAnd: true,
        data: null,
        keys: "StartDate",
        model: "ProcessTask",
        name: "上机时间",
        searchType: "Date",
    },
    {
        arrayData: null,
        canAnd: true,
        data: null,
        keys: "EndDate",
        model: "ProcessTask",
        name: "下机时间",
        searchType: "Date",
    },
    {
        arrayData: null,
        canAnd: true,
        data: null,
        keys: "CreationTime",
        model: "ProcessTask",
        name: "创建时间",
        searchType: "Date"
    },
    {
        arrayData: null,
        canAnd: false,
        data: null,
        keys: "PartSN,PartName",
        model: "Part",
        name: "零件",
        searchType: "Like",
    },
    {
        arrayData: null,
        canAnd: false,
        data: null,
        keys: "ProjectSN",
        model: "Part.Project",
        name: "模具编号",
        searchType: "Search",
    },
    {
        arrayData: null,
        canAnd: false,
        data: null,
        keys: "UnitName",
        model: "Supplier",
        name: "加工点",
        searchType: "Search",
    },
    {
        arrayData: null,
        canAnd: false,
        data: null,
        keys: "ProcessTypeName",
        model: "ProcessType",
        name: "工序",
        searchType: "Search"
    },
    {
        arrayData: null,
        canAnd: false,
        data: null,
        keys: "FeeType",
        model: "ProcessTask",
        name: "计价方式",
        searchType: "Search"
    },
    {
        arrayData: null,
        canAnd: false,
        data: null,
        keys: "Poster",
        model: "ProcessTask",
        name: "开单人",
        searchType: "Search",
    },
    {
        arrayData: null,
        canAnd: false,
        data: null,
        keys: "ProjectCharger",
        model: "ProcessTask",
        name: "模具组长",
        searchType: "Search"
    },
    {
        arrayData: null,
        canAnd: false,
        data: null,
        keys: "CraftsMan",
        model: "ProcessTask",
        name: "工艺师",
        searchType: "Search"
    },
    {
        arrayData: null,
        canAnd: false,
        data: null,
        keys: "Verifier",
        model: "ProcessTask",
        name: "审核人",
        searchType: "Search"
    },
    {
        arrayData: null,
        canAnd: false,
        data: null,
        keys: "Checker",
        model: "ProcessTask",
        name: "检验人",
        searchType: "Search"
    },
    {
        canAnd: false,
        data: null,
        keys: "ActualHours",
        model: "ProcessTask",
        name: "工时",
        searchType: "Array"
    }
]