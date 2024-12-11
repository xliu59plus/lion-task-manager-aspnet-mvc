/*!
 * Start Bootstrap - Grayscale v7.0.6 (https://startbootstrap.com/theme/grayscale)
 * Copyright 2013-2023 Start Bootstrap
 * Licensed under MIT (https://github.com/StartBootstrap/startbootstrap-grayscale/blob/master/LICENSE)
 */

// Scripts
window.addEventListener("DOMContentLoaded", (event) => {
  // Navbar shrink function
  var navbarShrink = function () {
    const navbarCollapsible = document.body.querySelector("#mainNav");
    if (!navbarCollapsible) {
      return;
    }
    if (window.scrollY === 0) {
      navbarCollapsible.classList.remove("navbar-shrink");
    } else {
      navbarCollapsible.classList.add("navbar-shrink");
    }
  };

  // Shrink the navbar
  navbarShrink();

  // Shrink the navbar when page is scrolled
  document.addEventListener("scroll", navbarShrink);

  // Activate Bootstrap scrollspy on the main nav element
  const mainNav = document.body.querySelector("#mainNav");
  if (mainNav) {
    new bootstrap.ScrollSpy(document.body, {
      target: "#mainNav",
      rootMargin: "0px 0px -40%",
    });
  }

  // Collapse responsive navbar when toggler is visible
  const navbarToggler = document.body.querySelector(".navbar-toggler");
  const responsiveNavItems = [].slice.call(
    document.querySelectorAll("#navbarResponsive .nav-link")
  );
  responsiveNavItems.map(function (responsiveNavItem) {
    responsiveNavItem.addEventListener("click", () => {
      if (window.getComputedStyle(navbarToggler).display !== "none") {
        navbarToggler.click();
      }
    });
  });

  // Custom JavaScript logic (your script starts here)
  const steps = document.querySelectorAll(".form-step");
  const progressBar = document.getElementById("progressBar");
  let currentStep = 0;
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
    WhitePrice: "reviewWhitePrice",
    CMYKWhitePrice: "reviewCMYKWhitePrice",
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

    const artworkSpecialization = document.getElementById(
      "artworkSpecialization"
    )?.value;
    const artworkOtherInput =
      document.getElementById("artworkOther")?.value || "N/A";
    document.getElementById("reviewArtworkSpecialization").textContent =
      artworkSpecialization === "Other"
        ? artworkOtherInput
        : artworkSpecialization || "N/A";

    document.getElementById("reviewTravelFeeAmount").textContent =
      document.getElementById("travelFeeAmount")?.value || "N/A";
  }

  document.querySelectorAll(".next-btn").forEach((btn) => {
    btn.addEventListener("click", () => {
      if (currentStep < steps.length - 1) {
        if (currentStep === steps.length - 2) {
          populateReview();
        }
        currentStep++;
        showStep(currentStep);
      }
    });
  });

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
});
