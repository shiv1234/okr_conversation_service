using System;
using System.Collections.Generic;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Adapters.BaseAdapter;
using Xunit;

namespace OkrConversationService.Infrastructure.Tests.BaseAdapter
{
    public class CommonBaseTest
    {
        [Fact]
        public void GetPayloadStatus_IsSuccessFalse()
        {
            var objModelStateDictionary = new ModelStateDictionary();
            var objPayloadString = new Payload<string>();
            //Act
            var objCommonBase = new CommonBase();
            var response = objCommonBase.GetPayloadStatus(objPayloadString, objModelStateDictionary);
            //Assert
            Assert.NotNull(response);
            Assert.False(response.IsSuccess);
        }
        [Fact]
        public void GetPayloadStatus_WithError_IsSuccessFalse()
        {
            var objModelStateDictionary = new ModelStateDictionary();
            objModelStateDictionary.AddModelError("TestError", new AggregateException().Message);
            var objPayloadString = new Payload<string>();
            //Act
            var objCommonBase = new CommonBase();
            var response = objCommonBase.GetPayloadStatus(objPayloadString, objModelStateDictionary);
            //Assert
            Assert.NotNull(response);
            Assert.False(response.IsSuccess);
        }
        [Fact]
        public void GetPayloadStatus_WithListValidationFailureEmpty_IsSuccessTrue()
        {
            var objListValidationFailure = new List<ValidationFailure>();
            var objPayloadString = new Payload<string>();
            //Act
            var objCommonBase = new CommonBase();
            var response = objCommonBase.GetPayloadStatus(objPayloadString, objListValidationFailure);
            //Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
        }
        [Fact]
        public void GetPayloadStatus_WithListValidationFailure_IsSuccessFalse()
        {
            var objListValidationFailure = new List<ValidationFailure>() {
            new ValidationFailure("test", "Empty")
            };
            var objPayloadString = new Payload<string>();
            //Act
            var objCommonBase = new CommonBase();
            var response = objCommonBase.GetPayloadStatus(objPayloadString, objListValidationFailure);
            //Assert
            Assert.NotNull(response);
            Assert.False(response.IsSuccess);
            Assert.Equal("Error", response.MessageType);
        }
    }
}
