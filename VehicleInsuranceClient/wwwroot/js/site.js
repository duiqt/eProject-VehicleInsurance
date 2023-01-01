var initialLoad = true;
$(document).ready(function () {
    var apiurl = "https://localhost:7008/api";

    // **** Toann Zone ****
    checkToKeepValuesOfInputFields();

    $("#VehicleName").change(function () {
        // When user change VehicleName field, change Model and Version values
        changeVehicleModelOptions($(this).val());
        changeVehicleVersionOptions($("#VehicleModel").val());
    });
    $("#VehicleModel").change(function () {
        changeVehicleVersionOptions($(this).val());
    });
    function checkAgreementBox() {
        if ($("#checkBoxAgree").is(":checked") == false) {
            $("#checkBoxAgreeErr").removeClass("d-none");
        } else {
            $("#checkBoxAgreeErr").addClass("d-none");
        }
    }
    // Checkbox for agreement of certificate contract
    $("#formContract").on("submit", function (e) {

        if ($("#checkBoxAgree").is(":checked") == false) {
            e.preventDefault();
            checkAgreementBox();
        }

    });
    $("#checkBoxAgree").change(function () {
        checkAgreementBox();
    });
    if (initialLoad) {
        $("#checkBoxAgreeErr").addClass("d-none");
    }

    // Annouce if customer not make an estimate yet
    if ($("#estimateNoErrMessage").length) {
        alert($("#estimateNoErrMessage").val());
    }
    // Annouce if image prove has problems
    if ($("#ContractErrMessage").length) {
        alert($("#ContractErrMessage").val());
    }

    // **** End of Toan Zone ****


    // **** Ngan Zone ****
    $.ajax({
        url: apiurl + "/Policy/GetPolicies",
        type: "GET",
        data: {},
        success: function (result) {
            GetViewHomePolicies(result);
        }
    });
    // **** End of Ngan Zone ****

    initialLoad = false;
}); // End of document.ready()

// **** Toann Functions Zone

function changeVehicleModelOptions(name) {
    //// This function is to generating options of VehicleModel 
    //// based on event "change" of VehicleName field
    $('#VehicleModel').prop('selectedIndex', 0);
    $('#VehicleVersion').prop('selectedIndex', 0);
    $("#VehicleModel > option").show();
    $("#VehicleModel > option").not("." + name).hide();
}
function changeVehicleVersionOptions(model) {
    //// This function is to generating options of VehicleVersion 
    //// based on event "change" of VehicleName / VehicleModel fields
    if (model != "") {
        $('#VehicleVersion').prop('selectedIndex', 0);
        $("#VehicleVersion > option").show();
        $("#VehicleVersion > option").not("." + model).hide();
    } else {
        $('#VehicleModel').prop('selectedIndex', 0);
        $('#VehicleVersion').prop('selectedIndex', 0);
        $("#VehicleVersion > option").hide();
    }
}
function checkToKeepValuesOfInputFields() {
    //// This function is to check Vehicle Name/Model/Version models Has Values or Not
    //// based on event "1st visit" or did get an estimate.
    let name = $("#VehicleName").val();
    let model = $("#VehicleModel").val();
    let version = $("#VehicleVersion").val();
    removeDuplicateValues();
    if (name == '' && model == '' && version == '') {
        // Hide all of the <select>'s options Id=VehicleVersion
        $("#VehicleVersion > option").hide();
    } else {
        $("#VehicleName").val(name);
        $("#VehicleModel").val(model);
        $("#VehicleVersion").val(version);
    }
}
function removeDuplicateValues() {
    // This function is to remove duplicated values of fields VehicleName/Model/Version
    let vehicleNames = {};
    $("#VehicleName > option").each(function () {
        if (vehicleNames[this.text]) {

            $(this).remove();
        } else {
            vehicleNames[this.text] = this.value;
        }
    });
    let vehicleModels = {};
    $("#VehicleModel > option").each(function () {
        if (vehicleModels[this.text]) {
            $(this).remove();
        } else {

            vehicleModels[this.text] = this.value;
            // In each Vehicle Models, add Class having same value of VehicleName
            $(this).addClass(this.text.split("_")[0]);
            this.text = this.text.split("_")[1];

        }
    });
    let vehicleVersions = {};
    $("#VehicleVersion > option").each(function () {
        if (vehicleVersions[this.text]) {
            $(this).remove();
        } else {
            vehicleVersions[this.text] = this.value;
            // In each Vehicle Version, add Class having same value of VehicleModel
            $(this).addClass(this.text.split("_")[0]);
            this.text = this.text.split("_")[1];

        }
    });
}



// **** End of Toann Functions Zone



// **** Ngan Zone

function GetViewHomePolicies(policytList) {
    $.ajax({
        url: "/Policy/GetPolicies",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(policytList),
        success: function (dataView) {
            $("#policies-content").html(dataView);
        }
    });
}

// **** End of Ngan Zone