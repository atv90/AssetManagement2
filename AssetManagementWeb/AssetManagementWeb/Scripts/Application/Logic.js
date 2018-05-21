/// <reference path="../typings/jquery/jquery.d.ts" />
var AssignLocationModel = /** @class */ (function () {
    function AssignLocationModel() {
    }
    return AssignLocationModel;
}());
function initAssetAssignment() {
    //alustetaan data
    $("#AssignAssetButton").click(function () {
        var locationCode = $("#LocationCode").val();
        var assetCode = $("#AssetCode").val();
        alert("L: " + locationCode + ", A: " + assetCode);
        var data = new AssignLocationModel();
        data.LocationCode = locationCode;
        data.AssetCode = assetCode;
        // lähetetään JSON-muotoista dataa palvelimelle ajax-komennolla
        $.ajax({
            type: "POST",
            //url:in pitää viitata oikeaan kontrolleriin ja siellä olevaan kohtaan
            url: "/Asset/AssignLocation",
            data: JSON.stringify(data),
            contentType: "application/json",
            success: function (data) {
                if (data.success == true) {
                    alert("Asset successfully assigned.");
                }
                else {
                    alert("There was an error: " + data.error);
                }
            },
            dataType: "json"
        });
    });
}
//# sourceMappingURL=Logic.js.map