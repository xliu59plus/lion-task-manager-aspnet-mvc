// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
  const steps = document.querySelectorAll(".form-step");
  const progressBar = document.getElementById("progressBar");
  let currentStep = 0;
  const fieldMapping = {
    // Business Information
    CompanyName: "reviewCompanyName",
    EIN: "reviewEIN",

    // Address Information
    addressInput: "reviewFullAddress",
    firstLine: "reviewFirstLine",
    secondLine: "reviewSecondLine",
    city: "reviewCity",
    stateProvince: "reviewStateProvince",
    zipCode: "reviewZipCode",

    // Work Preferences
    WallpenMachineModel: "reviewWallpenMachineModel",
    WallpenSerialNumber: "reviewWallpenSerialNumber",

    // Pricing Information
    CMYKPrice: "reviewCMYKPrice",
    WhitePrice: "reviewWhitePrice",
    CMYKWhitePrice: "reviewCMYKWhitePrice",

    // Social Media Links
    FacebookLink: "reviewFacebookLink",
    TikTokLink: "reviewTikTokLink",
  };
  function showStep(index) {
    steps.forEach((step, i) => {
      step.classList.toggle("d-none", i !== index);
    });
    const stepCount = steps.length;
    progressBar.style.width = `${((index + 1) / stepCount) * 100}%`;
    progressBar.textContent = `Step ${index + 1} of ${stepCount}`;
  }

  // Review
  function populateReview() {
    Object.entries(fieldMapping).forEach(([formFieldId, reviewFieldId]) => {
      const formField = document.getElementById(formFieldId);
      const reviewField = document.getElementById(reviewFieldId);

      if (formField && reviewField) {
        reviewField.textContent = formField.value || "N/A";
      }
    });

    document.getElementById("reviewPrintsWhite").textContent =
      document.querySelector('input[name="PrintsWhite"]:checked')?.value ||
      "N/A";
    document.getElementById("reviewSupportsCMYK").textContent =
      document.querySelector('input[name="SupportsCMYK"]:checked')?.value ||
      "N/A";
    document.getElementById("reviewTravelOver100").textContent =
      document.querySelector('input[name="travelOver100"]:checked')?.value ||
      "N/A";
    document.getElementById("reviewChargeTravelFees").textContent =
      document.querySelector('input[name="chargeTravelFees"]:checked')?.value ||
      "N/A";

    // Artwork Specialization
    const artworkSpecialization = document.getElementById(
      "artworkSpecialization"
    )?.value;
    const artworkOtherInput =
      document.getElementById("artworkOther")?.value || "N/A";
    document.getElementById("reviewArtworkSpecialization").textContent =
      artworkSpecialization === "Other"
        ? artworkOtherInput
        : artworkSpecialization || "N/A";

    // Travel Fee
    document.getElementById("reviewTravelFeeAmount").textContent =
      document.getElementById("travelFeeAmount")?.value || "N/A";
  }

  // next step button
  document.querySelectorAll(".next-btn").forEach((btn) => {
    btn.addEventListener("click", () => {
      if (currentStep < steps.length - 1) {
        // 如果进入 Review 步骤，填充信息
        if (currentStep === steps.length - 2) {
          populateReview();
        }
        currentStep++;
        showStep(currentStep);
      }
    });
  });

  // preview button
  document.querySelectorAll(".prev-btn").forEach((btn) => {
    btn.addEventListener("click", () => {
      if (currentStep > 0) {
        currentStep--;
        showStep(currentStep);
      }
    });
  });

  showStep(currentStep);

  const artworkSpecializationSelect = document.querySelector(
    '[name="ArtworkSpecialization"]'
  );
  const artworkOtherInput = document.getElementById("artworkOther");
  const travelFeeYes = document.getElementById("travelFeeYes");
  const travelFeeNo = document.getElementById("travelFeeNo");
  const travelFeeDetails = document.getElementById("travelFeeDetails");

  // Show/hide travel fee details
  if (travelFeeYes && travelFeeNo) {
    travelFeeYes.addEventListener("click", () => {
      travelFeeDetails.style.display = "block";
    });
    travelFeeNo.addEventListener("click", () => {
      travelFeeDetails.style.display = "none";
    });
  }

  // Show/hide "Other" input for Artwork Specialization
  if (artworkSpecializationSelect) {
    artworkSpecializationSelect.addEventListener("change", (event) => {
      if (event.target.value === "Other") {
        artworkOtherInput.classList.remove("d-none");
      } else {
        artworkOtherInput.classList.add("d-none");
        artworkOtherInput.value = ""; // Clear "Other" input if not used
      }
    });
  }
});
