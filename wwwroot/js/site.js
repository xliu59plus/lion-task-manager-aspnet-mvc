﻿/*!
 * Start Bootstrap - Grayscale v7.0.6 (https://startbootstrap.com/theme/grayscale)
 * Copyright 2013-2023 Start Bootstrap
 * Licensed under MIT (https://github.com/StartBootstrap/startbootstrap-grayscale/blob/master/LICENSE)
 */

// Scripts
window.addEventListener("DOMContentLoaded", () => {
    // 获取所有步骤元素和进度条
    window.currentStep = 0;
    const steps = document.querySelectorAll(".form-step");
    const progressBar = document.getElementById("progressBar");
    // 字段映射，用于填充 Review 页
    const fieldMapping = {
        CompanyName: "reviewCompanyName",
        EIN: "reviewEIN",
        addressInput: "reviewFullAddress",
        firstLine: "reviewFirstLine",
        secondLine: "reviewSecondLine",
        city: "reviewCity",
        stateProvince: "reviewStateProvince",
        zipCode: "reviewZipCode",
        WallpenMachineModel: "reviewWallpenMachineModel",
        WallpenSerialNumber: "reviewWallpenSerialNumber",
        CMYKPrice: "reviewCMYKPrice",
        WhiteColorPrice: "reviewDoesPrintWhiteColor",
        CMYKWhiteColorPrice: "reviewCMYKWhiteColorPrice",
        FacebookLink: "reviewFacebookLink",
        TikTokLink: "reviewTikTokLink",
    };

    // 显示当前步骤
    function showStep(index) {
        steps.forEach((step, i) => {
            step.classList.toggle("d-none", i !== index);
        });
        const stepCount = steps.length;
        progressBar.style.width = `${((index + 1) / stepCount) * 100}%`;
        progressBar.textContent = `Step ${index + 1} of ${stepCount}`;
    }

    // 填充 Review 页的数据
    function populateReview() {
        Object.entries(fieldMapping).forEach(([formFieldId, reviewFieldId]) => {
            const formField = document.getElementById(formFieldId);
            const reviewField = document.getElementById(reviewFieldId);

            if (formField && reviewField) {
                reviewField.textContent = formField.value || "N/A";
            }
        });

        // 填充单选按钮的值
        const reviewDoesPrintWhiteColor = document.getElementById("reviewDoesPrintWhiteColor");
        if (reviewDoesPrintWhiteColor) {
            reviewDoesPrintWhiteColor.textContent =
                document.querySelector('input[name="DoesPrintWhiteColor"]:checked')?.value || "N/A";
        }

        const reviewSupportsCMYK = document.getElementById("reviewSupportsCMYK");
        if (reviewSupportsCMYK) {
            reviewSupportsCMYK.textContent =
                document.querySelector('input[name="SupportsCMYK"]:checked')?.value || "N/A";
        }

        const reviewMaxTravelDistanceMiles = document.getElementById("reviewMaxTravelDistanceMiles");
        if (reviewMaxTravelDistanceMiles) {
            reviewMaxTravelDistanceMiles.textContent =
                document.querySelector('input[name="MaxTravelDistanceMiles"]:checked')?.value || "N/A";
        }

        const reviewChargeTravelFeesOverLimit = document.getElementById("reviewChargeTravelFeesOverLimit");
        if (reviewChargeTravelFeesOverLimit) {
            reviewChargeTravelFeesOverLimit.textContent =
                document.querySelector('input[name="DoesChargeTravelFeesOverLimit"]:checked')?.value || "N/A";
        }

        // Artwork Specialization
        const artworkSpecialization = document.getElementById("artworkSpecialization")?.value;
        const artworkOtherInput = document.getElementById("artworkOther")?.value || "N/A";
        const reviewArtworkSpecialization = document.getElementById("reviewArtworkSpecialization");
        if (reviewArtworkSpecialization) {
            reviewArtworkSpecialization.textContent =
                artworkSpecialization === "Other" ? artworkOtherInput : artworkSpecialization || "N/A";
        }

        // Travel Fee Amount
        const reviewTravelFeeOverLimit = document.getElementById("reviewTravelFeeOverLimit");
        if (reviewTravelFeeOverLimit) {
            reviewTravelFeeOverLimit.textContent =
                document.getElementById("travelFeeAmount")?.value || "N/A";
        }
    }

    // 绑定 Next 按钮
    document.querySelectorAll(".next-btn").forEach((btn) => {
        btn.addEventListener("click", () => {
            if (currentStep < steps.length - 1) {
                if (currentStep === steps.length - 2) {
                    populateReview(); // 调用填充数据的函数
                }
                currentStep++;
                showStep(currentStep);
            }
        });
    });

    // 绑定 Previous 按钮
    document.querySelectorAll(".prev-btn").forEach((btn) => {
        btn.addEventListener("click", () => {
            if (currentStep > 0) {
                currentStep--;
                showStep(currentStep);
            }
        });
    });

    // 特殊字段逻辑
    const artworkSpecializationSelect = document.getElementById("artworkSpecialization");
    const artworkOtherInput = document.getElementById("artworkOther");
    const travelFeeYes = document.getElementById("travelFeeYes");
    const travelFeeNo = document.getElementById("travelFeeNo");
    const travelFeeDetails = document.getElementById("travelFeeDetails");

    if (travelFeeYes && travelFeeNo) {
        travelFeeYes.addEventListener("click", () => {
            travelFeeDetails.style.display = "block";
        });
        travelFeeNo.addEventListener("click", () => {
            travelFeeDetails.style.display = "none";
        });
    }

    if (artworkSpecializationSelect) {
        artworkSpecializationSelect.addEventListener("change", (event) => {
            if (event.target.value === "Other") {
                artworkOtherInput.classList.remove("d-none");
            } else {
                artworkOtherInput.classList.add("d-none");
                artworkOtherInput.value = "";
            }
        });
    }

    // 初始化显示第一步
    showStep(currentStep);
});

