// 这是自动生成的类

using System;
using System.Collections.Generic;

namespace OLiOYouxiAttributes.Editor
{
    static public class DPropertyValidator
    {
        static private  Dictionary<Type, APropertyValidator> validatorsByAttributeType;

        static DPropertyValidator()
        {
            validatorsByAttributeType = new Dictionary<Type, APropertyValidator>();
            validatorsByAttributeType[typeof(MaxValueAttribute)] = new MaxValuePropertyValidator();
            validatorsByAttributeType[typeof(MinValueAttribute)] = new MinValuePropertyValidator();
            validatorsByAttributeType[typeof(RequiredAttribute)] = new RequiredPropertyValidator();
            validatorsByAttributeType[typeof(ValidateInputAttribute)] = new ValidateInputPropertyValidator();
            
        }

        static public APropertyValidator GetValidatorForAttribute(Type attributeType)
        {
            APropertyValidator validator;
            if (validatorsByAttributeType.TryGetValue(attributeType, out validator))
            {
                return validator;
            }
            else
            {
                return null;
            }
        }
    }
}

